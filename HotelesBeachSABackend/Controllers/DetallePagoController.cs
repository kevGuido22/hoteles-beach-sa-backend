using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePagoController : Controller
    {

        private readonly DbContextHotelBeachSA _context = null;

        public DetallePagoController(DbContextHotelBeachSA context)
        {
            _context = context;
        }


        [HttpPost("Crear")]
        [Authorize]
        public async Task<IActionResult> Crear(DetallePago detallePago)
        {
            if (detallePago == null)
            {
                return StatusCode(400, "Debe ingresar la información del detalle de pago.");
            }

            try
            {
                detallePago.Id = 0;
                await _context.DetallesPagos.AddAsync(detallePago);
                await _context.SaveChangesAsync();

                return StatusCode(201, detallePago);
            }
            catch (Exception ex)
            {
                return StatusCode(400, "El detalle de pago no se logro registrar correctamente");
            }
        }
    }
}
