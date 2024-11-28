using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using HotelesBeachSABackend.Models.Custom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesPermisosController : Controller
    {

        private readonly DbContextHotelBeachSA _context = null;

        public RolesPermisosController(DbContextHotelBeachSA context)
        {
            _context = context;
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado()
        {
            var rolesPermisos = await _context.RolesPermisos
            .Include(rp => rp.Rol)          // Incluye la información del Rol
            .Include(rp => rp.Permiso)      // Incluye la información del Permiso
            .Select(rp => new RolPermisoDTO // Proyecta los datos al DTO
            {
                RolPermisoId = rp.Id,
                RolId = rp.RolId,
                RolName = rp.Rol.Name,     // Asumiendo que la propiedad se llama "Nombre"
                PermisoId = rp.PermisoId,
                PermisoName = rp.Permiso.Name // Asumiendo que la propiedad se llama "Nombre"
            })
            .ToListAsync();

            return StatusCode(200, rolesPermisos);
        }

        

    }
}
