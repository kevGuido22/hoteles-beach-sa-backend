using Microsoft.EntityFrameworkCore;
using HotelesBeachSABackend.Models;

namespace HotelesBeachSABackend.Data
{
    public class DbContextHotelBeachSA : DbContext
    {
        public DbContextHotelBeachSA(DbContextOptions<DbContextHotelBeachSA> options) : base(options)
        {
        }

        public DbSet<DetallePago> DetallesPagos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<FormaPago> FormasPagos { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Reservacion> Reservaciones { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolPermiso> RolesPermisos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioRol> UsuariosRoles { get; set; }

    }
}