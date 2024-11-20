using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetesController : ControllerBase
    {
        private readonly DbContextHotelBeachSA _context = null;

        public PaquetesController(DbContextHotelBeachSA context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public List<Paquete> GetAll()
        {
            List<Paquete> list = null;
            list = _context.Paquetes.ToList();
            return list;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Paquete paquete)
        {
            if (paquete == null)
            {
                return BadRequest("Debe ingresar la información del paquete.");
            }
            if (paquete.CostoPersona == 0)
            {
                return BadRequest("El costo por persona debe ser mayor a 0");
            }

            if (paquete.PrimaReserva <= 0 || paquete.PrimaReserva >= 100)
            {
                return BadRequest("La prima debe ser un porcentaje entre 1% y 100. Inténtelo de nuevo");
            }
            if (paquete.Mensualidades == 0)
            {
                return BadRequest("La mensualidad debe ser mayor a 0"); 
            }
            try
            {
                await _context.Paquetes.AddAsync(paquete);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = $"El paquete '{paquete.Nombre}' se registró de manera exitosa."
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error al registrar el paquete '{paquete.Nombre}'.",
                    detalle = ex.InnerException?.Message ?? ex.Message
                }); 
            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al registrar el paquete '{paquete.Nombre}'.",
                    detalle = ex.Message
                });
            }
        }

    }
}
