using System.ComponentModel.DataAnnotations;

namespace HotelesBeachSABackend.Models
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe de ingresar el nombre del permiso")]
        public string Name { get; set; }

        //propiedad de navegacion
        public ICollection<RolPermiso>? RolPermiso { get; set; }
    }
}
