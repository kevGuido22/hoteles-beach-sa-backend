using Microsoft.EntityFrameworkCore;
using HotelesBeachSABackend.Models;

namespace HotelesBeachSABackend.Data
{
    public class DbContextHotelBeachSA : DbContext
    {
        public DbContextHotelBeachSA(DbContextOptions<DbContextHotelBeachSA> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}