using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelesBeachSABackend.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe ingresar los digitos de la cedula")]
        [MaxLength(9)]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "Debe de indicar el tipo de cedula")]
        public string Tipo_Cedula { get; set; }

        [Required(ErrorMessage = "Debe de ingresar el nombre completo")]
        [MinLength(6, ErrorMessage = "El nombre completo debe tener al menos 6 caracteres.")]
        [MaxLength(100, ErrorMessage = "El nombre completo no puede exceder los 100 caracteres.")]
        public string Nombre_Completo { get; set; }
        
        [Required(ErrorMessage = "Debe de ingresar el numero de telefono")]
        [StringLength(20, ErrorMessage = "El teléfono no puede tener más de 20 caracteres.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Debe de ingresar la direccion")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Debe de ingresar el correo electronico")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe de ingresar la contraseña")]
        [Column(TypeName = "VARCHAR(255)")]
        public string Password { get; set; }

        //propiedad de navegacion
        [JsonIgnore] // Esto evita que la propiedad sea serializada
        public ICollection<UsuarioRol>? UsuarioRol { get; set; }
    }
}
