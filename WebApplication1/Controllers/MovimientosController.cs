using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTOS;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{

    [ApiController]
    [Route("api/movimientos")]
    public class MovimientosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public MovimientosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("GetBalance")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaBalance>> GetBalance()
        {
           
            var tarjetaC = User.Claims.Where(c => c.Type == "numeroTarjeta").FirstOrDefault();
            var tarjeta = tarjetaC.Value;
            var usuario = context.Usuarios.FirstOrDefault(u => u.NumeroTarjeta == tarjeta);

            RespuestaBalance respuestaBalance = new RespuestaBalance();
            respuestaBalance.Balance = GetBalance(usuario);

            Operacion operacion = new Operacion() {
                Codigo = Guid.NewGuid().ToString(),
                DateTime = DateTime.Now,
                Movimiento = null,
                Usuario = usuario,
                TipoOperacion = "GetBalance"
            };

            context.Add(operacion);
            await context.SaveChangesAsync();


            return respuestaBalance;
        }

        [HttpPost("Retiro")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Operacion>> Retiro( decimal retiro) 
        {
            var tarjetaC = User.Claims.Where(c => c.Type == "numeroTarjeta").FirstOrDefault();
            var tarjeta = tarjetaC.Value;
            var usuario = context.Usuarios.FirstOrDefault(u => u.NumeroTarjeta == tarjeta);
            var balance = GetBalance(usuario);

            if (retiro > balance)
            {
                return Ok("No tiene suficiente dinero para retirar.");
            }
            else
            {
                Movimiento movimiento = new Movimiento() { 
                    Cantidad = -retiro,  
                    TipoMovimiento = "Negativo",
                    Usuario = usuario

                };
                context.Add(movimiento);
                await context.SaveChangesAsync();


                Operacion operacion = new Operacion()
                {
                    Codigo = Guid.NewGuid().ToString(),
                    DateTime = DateTime.Now,
                    Movimiento = movimiento,
                    Usuario = usuario,
                    TipoOperacion = "Retiro"
                };

                context.Add(operacion);
                await context.SaveChangesAsync();
                return operacion;
            }
        }

        private decimal GetBalance(Usuario usuario) {
            
            return  context.Movimientos.Where(m => m.Usuario == usuario).Sum(m => m.Cantidad);
        }

    }
}
