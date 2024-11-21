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

        [HttpGet("Crear")]
        public async Task<IActionResult> Crear([FromBody]Factura factura)
        {
            if(factura == null)
            {
                return BadRequest("Datos inválidos."); 
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            if (factura.FormaPagoId != null &&
                !await _context.FormasPagos.AnyAsync(f => f.Id == factura.FormaPagoId))
            {
                return BadRequest("La forma de pago asociada no existe.");
            }

            if (factura.ReservacionId != null &&
                !await _context.Reservaciones.AnyAsync(r => r.Id == factura.ReservacionId))
            {
                return BadRequest("La reservación asociada no existe.");
            }
            try
            {
                await _context.Facturas.AddAsync(factura);
                await _context.SaveChangesAsync(); 
                return Ok(new
                {
                    message =  $"la factura '{factura.Id}' se registró de manera exitosa"
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error al registrar la factura '{factura.Id}'",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al registrar la factura",
                    detalle = ex.Message
                });
            }
        }
    }
}
