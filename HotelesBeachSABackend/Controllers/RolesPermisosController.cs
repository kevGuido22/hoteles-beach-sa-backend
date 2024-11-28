using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using HotelesBeachSABackend.Models.Custom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            .Include(rp => rp.Rol)          //  JOIN
            .Include(rp => rp.Permiso)      // JOIN
            .Select(rp => new RolPermisoDTO // SELECT
            {
                RolPermisoId = rp.Id,
                RolId = rp.RolId,
                RolName = rp.Rol.Name,
                PermisoId = rp.PermisoId,
                PermisoName = rp.Permiso.Name
            })
            .ToListAsync();

            return StatusCode(200, rolesPermisos);
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(int rolPermisoId)
        {
            var rolPermiso = await _context.RolesPermisos
                .Include(rp => rp.Rol)          // JOIN
                .Include(rp => rp.Permiso)      // JOIN
                .Where(rp => rp.Id == rolPermisoId) // WHERE
                .Select(rp => new RolPermisoDTO // SELECT
                {
                    RolPermisoId = rp.Id,
                    RolId = rp.RolId,
                    RolName = rp.Rol.Name,
                    PermisoId = rp.PermisoId,
                    PermisoName = rp.Permiso.Name
                })
                .FirstOrDefaultAsync(); // Obtén el primer registro 

            if (rolPermiso == null)
            {
                return NotFound(new { message = "El RolPermiso no fue encontrado." });
            }

            return StatusCode(200, rolPermiso);



        }
    }
}
