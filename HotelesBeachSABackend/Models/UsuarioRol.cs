using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelesBeachSABackend.Models
{
    public class UsuarioRol
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe de ingresar la llave foranea del modelo Rol")]
        [ForeignKey("Rol")]
        public int RolId { get; set; }

        [Required(ErrorMessage = "Debe de ingresar la llave foranea del modelo Usuario")]
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        //Propiedades de Navegacion
        //Permiten acceder a las entidades relacionadas
        [JsonIgnore]
        public Usuario Usuario { get; set; }
        [JsonIgnore]
        public Rol Rol { get; set; }
    }
}
