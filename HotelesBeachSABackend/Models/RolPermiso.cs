using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelesBeachSABackend.Models
{
    public class RolPermiso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe de ingresar la llave foranea del modelo Rol")]
        [ForeignKey("Rol")]
        public int RolId { get; set; }

        [Required(ErrorMessage = "Debe de ingresar la llave foranea del modelo Permiso")]
        [ForeignKey("Permiso")]
        public int PermisoId { get; set; }

        //Propiedades de Navegacion
        //Permiten acceder a las entidades relacionadas
        [JsonIgnore]
        public Permiso Permiso { get; set; }
        [JsonIgnore]
        public Rol Rol { get; set; }
    }
}
