using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
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


    }
}
