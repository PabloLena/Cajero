using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entidades;

namespace WebApplication1
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<Operacion> Operaciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
