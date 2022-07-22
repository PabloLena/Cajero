using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Entidades
{
    public class Movimiento
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }    
        public string TipoMovimiento { get; set; }
        [Required]
        public decimal Cantidad { get; set; }
       
    }
}
