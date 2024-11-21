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
        public async Task<IActionResult> Listado()
        {
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
                    message = $"Ocurrió un error al obtener la lista de facturas.",
                    detalle = ex.Message
                });
            }
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Factura factura)
        {
            if (factura == null)
            {
                return BadRequest("Datos inválidos.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (factura.ReservacionId != null &&
                !await _context.Reservaciones.AnyAsync(r => r.Id == factura.ReservacionId))
            {
                return BadRequest("La reservación asociada no existe.");
            }

            if (factura.FormaPagoId != null)
            {
                var formaPago = await _context.FormasPagos
                    .Where(f => f.Id == factura.FormaPagoId)
                    .Select(f => new { f.Id, f.IsPaymentDetailRequired })
                    .FirstOrDefaultAsync();

                if (formaPago == null)
                {
                    return BadRequest("La forma de pago asociada no existe.");
                }
                if(formaPago.IsPaymentDetailRequired == false && factura.DetallePagoId != 0)
                {
                    return BadRequest("Esta factura no requiere detalle de pago.");

                }

                bool detallePagoExiste = await _context.DetallesPagos
                    .AnyAsync(d => d.Id == factura.DetallePagoId);

                if (formaPago.IsPaymentDetailRequired == true)
                {
                    if (!detallePagoExiste)
                    {
                        return BadRequest("El detalle de pago asociado no existe.");
                    }
                }
            }
            try
            {
                await _context.Facturas.AddAsync(factura);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = $"la factura '{factura.Id}' se registró de manera exitosa"
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

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(int id)
        {
            if(id == null)
            {
                return BadRequest("Debe ingresar un ID válido.");
            }
            try
            {
                Factura factura = await _context.Facturas.SingleOrDefaultAsync(f => f.Id == id); 
                if(factura == null)
                {
                    return BadRequest($"No se encontó una factura con el ID {id}"); 
                }
                return Ok(factura); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al buscar la factura",
                    detalle = ex.Message
                });
            }
        }






    }
}
