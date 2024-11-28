using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
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

        [HttpGet("Buscar")]
        public async Task<IActionResult> Buscar(int usuarioRolId) 
        {
            var usuarioRol = await _context.UsuariosRoles
               .Include(rp => rp.Usuario)
               .Include(rp => rp.Rol)
               .Where(rp => rp.Id == usuarioRolId)
               .Select(rp => new UsuarioRolDTO
               {
                   UsuarioRolID = rp.Id,
                   UsuarioId = rp.Usuario.Id,
                   Email = rp.Usuario.Email,
                   NombreCompleto = rp.Usuario.Nombre_Completo,
                   RolId = rp.Rol.Id,
                   RolName = rp.Rol.Name
               })
            .FirstOrDefaultAsync();

            if (usuarioRol == null)
            {
                return StatusCode(404, "El UsuarioRol no fue encontrado");
            }

            return StatusCode(200, usuarioRol);
        }
    }
}
