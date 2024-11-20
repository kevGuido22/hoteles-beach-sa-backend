using HotelesBeachSABackend.Models;
using HotelesBeachSABackend.Models.Custom;

//usar el contexto
using HotelesBeachSABackend.Data;

//usar librerias ORM
using Microsoft.EntityFrameworkCore;

//librerias para JWT
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelesBeachSABackend.Services
{
    public class AutorizacionServices : IAutorizacionServices
    {
        //usar configuracion para appsetting
        private readonly IConfiguration _configuration;

        private readonly DbContextHotelBeachSA _context;
        public AutorizacionServices(IConfiguration configuration, DbContextHotelBeachSA context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<AutorizacionResponse> DevolverToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Email)
            };
            //se identifica al usuario que está solicitando la autorizacion
            //se valida su email y password
            var temp = await _context.Usuarios.FirstOrDefaultAsync(x => x.Email.Equals(usuario.Email) && x.Password.Equals(usuario.Password));

            if (temp == null)
            {
                return await Task.FromResult<AutorizacionResponse>(null);
            }

            string tokenCreado = GenerarToken(usuario.Email.ToString());

            var roles = await _context.UsuariosRoles
            .Where(ur => ur.UsuarioId == temp.Id)
            .Select(ur => new
            {
                RolNombre = ur.Rol.Name,
                Permisos = ur.Rol.RolPermiso.Select(rp => rp.Permiso.Name).ToList()
            }).ToListAsync();

            

            //return new AutorizacionResponse() { Token = tokenCreado, Resultado = true, Msj = "Ok" };
            return new AutorizacionResponse()
            {
                Token = tokenCreado,
                Resultado = true,
                Msj = "Ok",
                Roles = roles.Select(r => new RolPermisoResponse
                {
                    Rol = r.RolNombre,
                    Permisos = r.Permisos
                }).ToList()
            };
        }

        private string GenerarToken(string IDUser)
        {
            var key = _configuration.GetValue<string>("JwtSettings:key");

            //se convierte la key en un vector de bytes
            var KeyBytes = Encoding.ASCII.GetBytes(key);

            //se decalra la idnetidad que realiza el reclamo para la solicitud de autorizacion
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, IDUser));

            // se instancian las credendeciales del token llave que usa el algoritmo de cifrado
            var credencialesToken = new SigningCredentials(new SymmetricSecurityKey(KeyBytes), SecurityAlgorithms.HmacSha256Signature); // algoritmo del filtrado

            //se instancia el descriptor para el token 
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims, // se asigna la identidad
                Expires = DateTime.UtcNow.AddMinutes(3), // se agrega el tiempo de vida de 3 minutos para el token
                SigningCredentials = credencialesToken, // se asignan las credenciales
            };

            //se instacia el tokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();

            // se crea el token
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            //se escribe el token
            var tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;
        }
    }
}
