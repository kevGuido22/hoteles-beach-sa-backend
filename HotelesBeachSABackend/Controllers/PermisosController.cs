using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : Controller
    {
        private readonly DbContextHotelBeachSA _context = null;

        public PermisosController(DbContextHotelBeachSA context)
        {
            _context = context;
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado()
        {
            List<Permiso> permisos = new List<Permiso>();

            permisos = _context.Permisos.ToList();

            return StatusCode(200, permisos);
        }

        [HttpPost("Crear")]
        [Authorize]
        public async Task<IActionResult> Crear(Permiso permiso)
        {
            if (permiso == null)
            {
                return StatusCode(400, "Debe llenar los datos del permiso");
            }

            try
            {
                await _context.Permisos.AddAsync(permiso);

                await _context.SaveChangesAsync();

                return StatusCode(201, permiso);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear un Rol {ex.InnerException}");
            }
        }

        [HttpPut("Editar")]
        [Authorize]
        public async Task<IActionResult> Editar(Permiso permiso)
        {
            if (permiso == null)
            {
                return StatusCode(400, "Debe de llenar los datos del permiso");
            }

            Permiso permisoTemp = await _context.Permisos.FirstOrDefaultAsync(x => x.Id == permiso.Id);

            if (permisoTemp == null)
            {
                return StatusCode(404, "Este permiso no se encuentra registrado");
            }

            try
            {
                permisoTemp.Name = permiso.Name;

                _context.Permisos.Update(permisoTemp);

                await _context.SaveChangesAsync();

                return StatusCode(200, "Permiso editado correctamente");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al editar un Permiso: {ex.InnerException}");
            }

        }

        [HttpDelete("Eliminar")]
        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            Permiso permisoTemp = await _context.Permisos.FirstOrDefaultAsync(x => x.Id == id);

            if (permisoTemp == null)
            {
                return StatusCode(400, "No existe un permiso con este id");
            }

            try
            {
                _context.Permisos.Remove(permisoTemp);

                await _context.SaveChangesAsync();

                return StatusCode(200, "Permiso eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar un permiso: {ex.InnerException}");
            }
        }

    }
}
