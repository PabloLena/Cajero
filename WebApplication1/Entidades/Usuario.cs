using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplication1.Entidades
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [StringLength(16)]
        public string NumeroTarjeta { get; set; }
        public bool Bloqueada { get; set; }
        [Required]
        [JsonIgnore]
        public int Pin { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int IntentosLogin { get; set; }
    }
}
