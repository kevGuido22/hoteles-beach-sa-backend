using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using HotelesBeachSABackend.Models.Custom;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
        [Authorize]
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

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear(UsuarioRolDTO usuarioRol) 
        {
            Usuario usuarioTemp = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == usuarioRol.UsuarioId);

            if (usuarioTemp == null)
            {
                return StatusCode(400, $"No existe un usuario con este id {usuarioRol.UsuarioRolID}");
            }

            Rol rolTemp = await _context.Roles.FirstOrDefaultAsync(x => x.Id == usuarioRol.RolId);

            if (rolTemp == null)
            {
                return StatusCode(400, $"No existe un rol con este Id {usuarioRol.RolId}");
            }

            try
            {
                UsuarioRol usuarioRolTemp = new UsuarioRol
                {
                    Id = 0,
                    UsuarioId = usuarioTemp.Id,
                    RolId = rolTemp.Id,
                };

                _context.UsuariosRoles.Add(usuarioRolTemp);

                await _context.SaveChangesAsync();

                return StatusCode(201, usuarioRolTemp);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Error al agregar un UsuarioRol: {ex.InnerException}");
            }


        }
        

    }
}
