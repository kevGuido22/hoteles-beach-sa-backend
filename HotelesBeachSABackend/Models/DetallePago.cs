using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelesBeachSABackend.Models
{
    public class DetallePago
    {
        [Key]
        public int Id { get; set; }

        public string? NumeroCheque { get; set; }

        public string? NumeroTarjeta { get; set; }

        public string? Banco {  get; set; }
    }
}
