using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FormaPagoController : ControllerBase
    {
        private readonly DbContextHotelBeachSA _context = null;

        public FormaPagoController(DbContextHotelBeachSA context)
        {
            _context = context;
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado()
        {
            try
            {
                var list = await _context.FormasPagos.ToListAsync();

                if (list.Count == 0 || list == null)
                {
                    return NotFound("No hay formas de pago registradas.");
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Ocurrió un error al obtener la lista de formas de pago",
                    detalle = ex.Message
                });
            }
        }
        [HttpGet("ObtenerPaqueteAsync")]
        public async Task<FormaPago> ObtenerFormaPagoAsync(int id)
        {
            return await _context.FormasPagos.FirstOrDefaultAsync(f => f.Id == id);
        }

        

    }
}
