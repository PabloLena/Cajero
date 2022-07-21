using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entidades;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GenerosController: ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenerosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Genero>>> Get() {
            return await context.Generos.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> Get(int id)
        {
            var entidad = await context.Generos.FirstOrDefaultAsync(p => p.Id == id);
            if (entidad == null) return NotFound();
            return entidad;
        }


        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Genero genero) {

            context.Add(genero);
            await context.SaveChangesAsync();


            return Ok();
        }
    }
}
