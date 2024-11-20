using HotelesBeachSABackend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Models;
using Microsoft.EntityFrameworkCore;
namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservacionController : ControllerBase
    {
        private readonly DbContextHotelBeachSA _context = null;
        public ReservacionController(DbContextHotelBeachSA context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public List<Reservacion> GetAll()
        {
            List<Reservacion> list = null;
            list = _context.Reservaciones.ToList();
            return list;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(Reservacion reservacion)
        {




            if (reservacion == null)
            {
                return BadRequest("Debe ingresar la información de reservación.");
            }
            if (reservacion.UsuarioId == 0)
            {
                return BadRequest("El ID del usuario debe ser mayor a 0.");
            }
            if (reservacion.PaqueteId == 0)
            {
                return BadRequest("Debe ingresar el número de paquete.");
            }
            if (reservacion.CantidadPersonas == 0)
            {
                return BadRequest("La cantidad de personas debe ser mayor a 0.");
            }
            if (reservacion.PaqueteId != null)
            {
                bool paqueteExiste = _context.Paquetes
                    .Any(p => p.Id == reservacion.PaqueteId && p.IsEnabled == 1);

                if (!paqueteExiste)
                {
                    return BadRequest("El paquete no existe o no está habilitado.");
                }
            }
            try
            {
                await _context.Reservaciones.AddAsync(reservacion);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = $"La reservación '{reservacion.Id}' se registró de manera exitosa."

                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error al registrar la reservación '{reservacion.Id}'",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al registrar la reservación",
                    detalle = ex.Message
                });
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id) {
            string message = "";
            Reservacion tempReservacion = _context.Reservaciones.FirstOrDefault(x => x.Id == id);
            if (tempReservacion == null)
            {
                return BadRequest("El número de reservación no existe.");
            }
            if (tempReservacion != null)
            {
                _context.Reservaciones.Remove(tempReservacion);
                await _context.SaveChangesAsync();
                message = $"La reservación '{tempReservacion.Id}' se eliminó de manera exitosa.";
            }
            return Ok(new
            {
                message = message
            });
        }
        
        
    }
}
