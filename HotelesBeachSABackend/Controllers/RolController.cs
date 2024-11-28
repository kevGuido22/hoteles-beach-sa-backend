using HotelesBeachSABackend.Data;
using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : Controller
    {
        private readonly DbContextHotelBeachSA _context = null;

        public RolController(DbContextHotelBeachSA context)
        {
            _context = context;
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado()
        {
            List<Rol> roles = new List<Rol>();

            roles = _context.Roles.ToList();

            return StatusCode(200, roles);
        }

        [HttpPost("Crear")]
        [Authorize]
        public async Task<IActionResult> Crear(Rol rol) 
        {
            if (rol == null) {
                return StatusCode(400, "Debe llenar los datos del rol");
            }

            try
            {
                await _context.Roles.AddAsync(rol);

                await _context.SaveChangesAsync();

                return StatusCode(201, rol);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error al crear un Rol");
            }
        }
    }
}
