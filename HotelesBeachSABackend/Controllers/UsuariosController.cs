using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Data;
using HotelesBeachSABackend.Models;
using Microsoft.EntityFrameworkCore;
using HotelesBeachSABackend.Models.Custom;
using HotelesBeachSABackend.Services;
namespace HotelesBeachSABackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : Controller
    {
        private readonly DbContextHotelBeachSA _context = null;

        private readonly IAutorizacionServices _autorizacionServices;

        public UsuariosController(DbContextHotelBeachSA context, IAutorizacionServices autorizacionServices)
        {
            _context = context;
            _autorizacionServices = autorizacionServices;
        }

        [HttpGet("Listado")]
        public async Task<IActionResult> Listado()
        {
            List<Usuario> usuarios = null;

            try
            {
                usuarios = await _context.Usuarios.ToListAsync();

                return StatusCode(200, usuarios);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = $"Ocurrió un error al obtener la lista de usuarios",
                    detalle = ex.Message
                });
            }
        }

        [HttpPost("ValidarUsuario")]
        public async Task<IActionResult> ValidarUsuario(Usuario user)
        {
            var temp = _context.Usuarios.FirstOrDefault(x => x.Email.Equals(user.Email));

            Usuario userAuth = null;

            if (temp == null)
            {
                return NotFound();
            }

            if (!temp.Password.Equals(user.Password))
            {
                return NotFound();
            }

            userAuth = user;

            return Ok(userAuth);
        }

        [HttpPost]
        [Route("AutenticarPW")]
        public async Task<IActionResult> AutenticarPW(string email, string password)
        {
            //se valida el email y pw deben ser correctos
            var temp = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.Equals(email) && x.Password.Equals(password));

            if (temp == null)
            {
                //return Unauthorized();
                return Unauthorized(new AutorizacionResponse() { Token = "", Msj = "No autorizado", Resultado = false });
            }

            var autorizado = await _autorizacionServices.DevolverToken(temp);

            if (autorizado == null)
            {
                return Unauthorized(new AutorizacionResponse() { Token = "", Msj = "No autorizado", Resultado = false });
            }

            return Ok(autorizado);
        }

        [HttpPost("Agregar")]
        public async Task<IActionResult> Agregar(Usuario usuario)
        {
            if (usuario == null)
            {
                return StatusCode(400, "Debe completar todos los datos del usuario");
            }

            Usuario temp = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuario.Email);

            if (temp != null)
            {
                return StatusCode(400, "Ya existe un usuario registrado con este email");
            }

            try
            {
                usuario.Id = 0;

                _context.Add(usuario);

                await _context.SaveChangesAsync();

                //obtener el id del usuario registrado
                int idUsuario = usuario.Id;

                //asociar al usuario con un rol (General)
                UsuarioRol usuarioRol = new UsuarioRol { UsuarioId = idUsuario, RolId = 2};

                //guadar relacion en la BD
                _context.UsuariosRoles.Add(usuarioRol);
                await _context.SaveChangesAsync();

                return StatusCode(200, "Usuario registrado exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.InnerException);
            }


        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Editar(Usuario usuario)
        {

            Usuario temp = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == usuario.Email);

            if (temp == null)
            {
                return StatusCode(404, "Usuario no encontrado");
            }

            try
            {
                temp.Telefono = usuario.Telefono;
                temp.Direccion = usuario.Direccion;
                temp.Nombre_Completo = usuario.Nombre_Completo;
                temp.Tipo_Cedula = usuario.Tipo_Cedula;
                temp.Cedula = usuario.Cedula;

                _context.Update(temp);

                await _context.SaveChangesAsync();

                return StatusCode(200, "Usuario editado corectamente ");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.InnerException);
            }
        }

        [HttpDelete("Eliminar")]
        public async Task<IActionResult> Eliminar(string cedula)
        {
            Usuario temp = await _context.Usuarios.FirstOrDefaultAsync(x => x.Cedula == cedula);

            if (temp == null)
            {
                return StatusCode(400, "Usuario no encontrado");
            }

            try
            {
                _context.Remove(temp);

                await _context.SaveChangesAsync();

                return StatusCode(200, "Usuario elimiando exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.InnerException);
            }

        }

        [HttpGet("Buscar")]
        public Usuario Buscar(string email)
        {
            Usuario temp = null;

            temp = _context.Usuarios.FirstOrDefault(x => x.Email == email);

            return temp == null ? new Usuario() { Nombre_Completo = "No existe" } : temp;

        }

        [HttpGet("ObtenerUsuarioAsync")]
        public async Task<Usuario> ObtenerUsuarioAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);


        }
    }//fin clase
}
