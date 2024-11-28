using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using HotelesBeachSABackend.Models.Custom;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        //[Authorize]
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
                return StatusCode(404, "El RolPermiso no fue encontrado");
            }

            return StatusCode(200, rolPermiso);
        }

        [HttpPost("Crear")]
        [Authorize]
        public async Task<IActionResult> Crear(RolPermisoDTO rolPermisoDTO) 
        {
            Rol rolTemp = await _context.Roles.FirstOrDefaultAsync(x => x.Id == rolPermisoDTO.RolId);

            if(rolTemp == null)
            {
                return StatusCode(400, $"No existe un rol con este Id {rolPermisoDTO.RolId}");
            }

            Permiso permisoTemp = await _context.Permisos.FirstOrDefaultAsync(x => x.Id == rolPermisoDTO.PermisoId);

            if (permisoTemp == null)
            {
                return StatusCode(400, $"No existe un permiso con este Id {rolPermisoDTO.PermisoId}");
            }

            try
            {
                RolPermiso rolPermisoTemp = new RolPermiso
                {
                    Id = 0,
                    RolId = rolTemp.Id,
                    PermisoId = permisoTemp.Id,
                };

                _context.RolesPermisos.Add(rolPermisoTemp);

                await _context.SaveChangesAsync();

                return StatusCode(201, rolPermisoTemp);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al agregar un rolPermiso: {ex.InnerException}");
            }

        }

        [HttpDelete("Eliminar")]
        [Authorize]
        public async Task<IActionResult> Eliminar(int rolPermisoId) 
        { 
            RolPermiso rolPermiso = await _context.RolesPermisos.FirstOrDefaultAsync(x => x.Id == rolPermisoId);

            if (rolPermiso == null) 
            {
                return StatusCode(404, "No se encuentra este registro de rolPermiso");
            }

            try
            {
                _context.RolesPermisos.Remove(rolPermiso);

                await _context.SaveChangesAsync();

                return StatusCode(200, "RolPermiso eliminado");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al eliminar un rolPermiso: {ex.InnerException}");
            }
        }
    }
}
