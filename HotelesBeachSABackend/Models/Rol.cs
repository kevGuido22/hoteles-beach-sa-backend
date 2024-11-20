using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelesBeachSABackend.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe de ingresar el nombre del rol")]
        public string Name { get; set; }

        //propiedad de navegacion
        [JsonIgnore] // Esto evita que la propiedad sea serializada
        public ICollection<RolPermiso> RolPermiso { get; set; }
        [JsonIgnore] // Esto evita que la propiedad sea serializada
        public ICollection<UsuarioRol> UsuarioRol { get; set; }
    }
}
