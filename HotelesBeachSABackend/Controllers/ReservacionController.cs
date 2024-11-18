using HotelesBeachSABackend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Models;
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

        [HttpGet("GetAll")]
        public List<Factura> GetAll()
        {
            List<Factura> list = null;
            list = _context.Facturas.ToList();
            return list;
        }


    }
}
