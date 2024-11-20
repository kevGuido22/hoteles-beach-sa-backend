using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using Microsoft.EntityFrameworkCore; 
namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        private readonly DbContextHotelBeachSA _context = null; 

        public FacturaController(DbContextHotelBeachSA context)
        {
            _context = context;
        }

        [HttpGet("Listado")]
        public async  Task<IActionResult> Listado() {
            try
            {
                var list = await _context.Facturas.ToListAsync();

                if (list.Count == 0 || list == null)
                {
                    return NotFound("No hay facturas registradas.");
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Ocurrió un error al obtener la lista de facturas habilitadas",
                    detalle = ex.Message
                });
            }
        }

    }
}
