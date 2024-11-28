using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HotelesBeachSABackend.Models
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe de ingresar el nombre del permiso")]
        public string Name { get; set; }

        //propiedad de navegacion
        [JsonIgnore]
        public ICollection<RolPermiso>? RolPermiso { get; set; }
    }
}
