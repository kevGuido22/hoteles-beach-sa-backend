﻿using Microsoft.AspNetCore.Mvc;
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

        
        [HttpGet("ListadoCompleto")]
        public List<Paquete> ListadoCompleto()
        {
            List<Paquete> list = null;
            list = _context.Paquetes.ToList();
            return list;
        }

        [HttpGet("ListadoHabilitados")]
        public async Task<IActionResult> ListadoHabilitados()
        {
            try
            {
                var list = await _context.Paquetes
                    .Where(p => p.IsEnabled == 1)
                    .ToListAsync(); 
                if(list.Count == 0)
                {
                    return NotFound("No hay paquetes habilitados.");
                }
                return Ok(list); 
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Ocurrió un error al obtener la lista de paquetes habilitados",
                    detalle = ex.Message
                });
            }

        }

        [HttpGet("ObtenerPaqueteAsync")]
        public async Task<Paquete> ObtenerPaqueteAsync(int id)
        {
            return await _context.Paquetes.FirstOrDefaultAsync(f => f.Id == id);
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(int id)
        {
            if(id == null) //quiero que verifique si se esta enviando un string
            {
                return BadRequest("Debe ingresar un ID válido"); 
            }
            try
            {
                Paquete paquete = await _context.Paquetes.SingleOrDefaultAsync(x => x.Id == id); 
                if(paquete == null)
                {
                    return BadRequest($"No se encontró un paquete con el ID {id}"); 
                }
                return Ok(paquete);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Error inesperado al buscar el paquete",
                    detalle = ex.Message
                });
            }

        }
        
        [HttpPost("Crear")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
