using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOS;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IConfiguration configuration;

        public UsuariosController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }


        [HttpGet("{numeroTarjeta}")]
        public async Task<ActionResult<Usuario>> GetUserInfo(string numeroTarjeta)
        {
            var entidad =  await context.Usuarios
                .FirstOrDefaultAsync(p => p.NumeroTarjeta == numeroTarjeta) ;

            if (entidad == null)
            {
                return NotFound();
            }
            return entidad;
        }

        [HttpGet("/Login")]
        public async Task<ActionResult<RespuestaAutentificacion>> Login([FromQuery] string numeroTarjeta, int pin)
        {
            var entidad = await context.Usuarios
                .FirstOrDefaultAsync(p => p.NumeroTarjeta == numeroTarjeta && p.Pin == pin);        

            if (entidad == null)
            {              
                var entidad2 = await context.Usuarios
                .FirstOrDefaultAsync(p => p.NumeroTarjeta == numeroTarjeta);


                if (entidad2.Bloqueada)
                {
                    return Ok("Cuenta Bloqueada");
                }

                entidad2.IntentosLogin = entidad2.IntentosLogin + 1;

                if (entidad2.IntentosLogin >  3)
                {
                    entidad2.Bloqueada = true;
                }

                context.Entry(entidad2).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return NotFound("pin incorrecto");
            }
            else
            {
                if (entidad.Bloqueada)
                {
                    return Ok("Cuenta Bloqueada");
                }

                entidad.IntentosLogin = 0;
                context.Entry(entidad).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return ConstruirToken(entidad);
            }
          
        }

        private RespuestaAutentificacion ConstruirToken(Usuario usuario) {

            var claims = new List<Claim>() {
                new Claim("numeroTarjeta", usuario.NumeroTarjeta)
            };

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutentificacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken)
            };
        }


    }
}
