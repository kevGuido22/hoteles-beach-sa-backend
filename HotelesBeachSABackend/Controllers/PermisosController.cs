using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    }
}
