using System.ComponentModel.DataAnnotations;

namespace HotelesBeachSABackend.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
