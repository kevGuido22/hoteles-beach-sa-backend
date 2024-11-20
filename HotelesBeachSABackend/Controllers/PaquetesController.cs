using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("Listado")]
        public List<Paquete> Listado()
        {
            List<Paquete> list = null;
            list = _context.Paquetes.ToList();
            return list;
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear(Paquete paquete)
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
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al registrar el paquete '{paquete.Nombre}'.",
                    detalle = ex.Message
                });
            }
        }

        [HttpPut("CambiarHabilitado")]
        public async Task<IActionResult> CambiarHabilitado(Paquete paquete)
        {
            if (paquete == null)
            {
                return BadRequest("Datos invalidos. Ingrese un ID válido.");
            }

            try
            {
                Paquete tempPaquete = await _context.Paquetes.FindAsync(paquete.Id);

                if (tempPaquete == null)
                {
                    return BadRequest($"No se encontró un paquete con el ID {paquete.Id}.");
                }

                tempPaquete.IsEnabled = paquete.IsEnabled;
                _context.Paquetes.Update(tempPaquete);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = $"El paquete {tempPaquete.Nombre} ha sido {(tempPaquete.IsEnabled == 1 ? "habilitado" : "deshabilitado")}.",
                    paquete
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error al cambiar el estado del paquete",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al cambiar el estado",
                    detalle = ex.Message
                });
            }

        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar(Paquete tempPaquete)
        {
            if (tempPaquete == null)
            {
                return BadRequest("Datos inválidos.");
            }

            try
            {
                Paquete paqueteExistente = await _context.Paquetes.SingleOrDefaultAsync(x => x.Id == tempPaquete.Id);
                if (paqueteExistente == null)
                {
                    return NotFound($"El paquete con el ID {tempPaquete.Id} no existe");
                }


                paqueteExistente.Nombre = tempPaquete.Nombre;
                paqueteExistente.CostoPersona = tempPaquete.CostoPersona;
                paqueteExistente.PrimaReserva = tempPaquete.PrimaReserva;
                paqueteExistente.Mensualidades = tempPaquete.Mensualidades;

                _context.Paquetes.Update(paqueteExistente);
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    message = $"Paquete {paqueteExistente.Nombre} actualizado correctamente. Este método no cambia el estado de Habilitado o Desabilitado",
                    paqueteExistente
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error al actualizar el paquete '{tempPaquete.Nombre}'",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al actualizar el paquete",
                    detalle = ex.Message
                });
            }



        }
    }
}
