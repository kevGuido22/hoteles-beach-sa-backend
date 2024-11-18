using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace HotelesBeachSABackend.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Cedula { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Tipo_Cedula { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Nombre_Completo { get; set; }
        
        [Required]
        [Column(TypeName = "VARCHAR(20)")]
        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres.")]
        public string Telefono { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Direccion { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Password { get; set; }
    }
}
