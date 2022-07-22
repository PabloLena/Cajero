using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entidades
{
    public class Operacion
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public Movimiento Movimiento { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        public string Codigo { get; set; }
        
        public string TipoOperacion { get; set; }
    }
}
