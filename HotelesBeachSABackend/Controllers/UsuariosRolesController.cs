using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models.Custom;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosRolesController : Controller
    {
        private readonly DbContextHotelBeachSA _context = null;
        public UsuariosRolesController(DbContextHotelBeachSA context)
        {
            _context = context;
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado() 
        {
            var usuariosRoles = await _context.UsuariosRoles
                .Include(rp => rp.Usuario)
                .Include(rp => rp.Rol)
                .Select(rp => new UsuarioRolDTO 
                { 
                    UsuarioRolID = rp.Id,
                    UsuarioId = rp.Usuario.Id,
                    Email = rp.Usuario.Email,
                    NombreCompleto = rp.Usuario.Nombre_Completo,
                    RolId = rp.Rol.Id,
                    RolName = rp.Rol.Name
                })
                .ToListAsync();

            return StatusCode(200, usuariosRoles);
        }
    }
}
