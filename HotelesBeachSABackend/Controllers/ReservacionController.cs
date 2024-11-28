using HotelesBeachSABackend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("Listado")]
        public List<Reservacion> Listado()
        {
            List<Reservacion> list = null;
            list = _context.Reservaciones.ToList();
            return list;
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
                Reservacion reservacion = await _context.Reservaciones.SingleOrDefaultAsync(x => x.Id == id);
                if(reservacion == null)
                {
                    return BadRequest($"No se encontró una reservación con el ID {id}"); 
                }
                return Ok(reservacion); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al buscar la reservación",
                    detalle = ex.Message
                });
            }
        }
        [HttpGet("ObtenerReservacionAsync")]
        public async Task<Reservacion> ObtenerReservacionAsync(int id)
        {
            return await _context.Reservaciones.FirstOrDefaultAsync(f => f.Id == id);
        }

        [HttpPost("Crear")]
        [Authorize]
        public async Task<IActionResult> Crear(Reservacion reservacion)
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
            
            if(reservacion.UsuarioId != null)
            {
                bool usuarioExiste = _context.Usuarios
                    .Any(p => p.Id == reservacion.UsuarioId);
                if (!usuarioExiste)
                {
                    return BadRequest("El usuario no existe."); 
                }
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
                //return Ok(new
                //{
                //    message = $"La reservación '{reservacion.Id}' se registró de manera exitosa."

                //});
                return StatusCode(201, reservacion);
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

        [HttpDelete("Eliminar")]
        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
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

        [HttpPut("Editar")]
        [Authorize]
        public async Task<IActionResult> Editar(Reservacion tempReservacion)
        {
            if (tempReservacion == null)
            {
                return BadRequest("Datos inválidos.");
            }

            if (tempReservacion.PaqueteId != null)
            {
                bool paqueteExiste = _context.Paquetes
                    .Any(p => p.Id == tempReservacion.PaqueteId && p.IsEnabled == 1);

                if (!paqueteExiste)
                {
                    return BadRequest("El paquete no existe o no está habilitado.");
                }
            }

            try
            {
                Reservacion reservacionExistente = await _context.Reservaciones.SingleOrDefaultAsync(x => x.Id == tempReservacion.Id);
                if (reservacionExistente == null)
                {
                    return NotFound($"El paquete con el ID {tempReservacion.Id} no existe");
                }

                reservacionExistente.FechaInicio = tempReservacion.FechaInicio;
                reservacionExistente.FechaFin = tempReservacion.FechaFin;
                reservacionExistente.PaqueteId = tempReservacion.PaqueteId;
                reservacionExistente.CantidadPersonas = tempReservacion.CantidadPersonas;
                _context.Reservaciones.Update(reservacionExistente);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = $"La reservación {reservacionExistente.Id} se actualizó correctamente.",
                    reservacionExistente
                }); 
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error al actualizar la reservación '{tempReservacion.Id}'",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al actualizar la reservación",
                    detalle = ex.Message
                });
            }
        }
    }
}
