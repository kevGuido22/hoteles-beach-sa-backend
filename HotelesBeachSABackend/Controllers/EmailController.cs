using HotelesBeachSABackend.Models;
using HotelesBeachSABackend.Services;
using Microsoft.AspNetCore.Mvc;
using HotelesBeachSABackend.Controllers;
using Microsoft.EntityFrameworkCore;
using HotelesBeachSABackend.Data;
namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly InvoiceService invoiceService;
        private readonly DbContextHotelBeachSA _context = null;

        PaquetesController paquetesController;
        FacturaController facturaController;
        UsuariosController usuariosController;
        ReservacionController reservacionController; 
        public EmailController(IEmailService emailService, InvoiceService invoiceService, DbContextHotelBeachSA context)
        {
            this.emailService = emailService;
            this.invoiceService = invoiceService;
            _context = context;
        }

        //[HttpPost("Send")]
        //public async Task<IActionResult> Send(string email, string theme, string body)
        //{
        //    await emailService.SendEmail(email, theme, body);
        //    return Ok();
        //}

        [HttpPost("SendWithInvoice")]
        public async Task<IActionResult> SendWithInvoice(string email, int idFactura, string formaPago)
        {
            Reservacion tempReservacion = null;
            Paquete tempPaquete = null;
            Factura tempFactura =  await _context.Facturas.FirstOrDefaultAsync(f => f.Id == idFactura);
            Usuario tempUsuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            
            if (tempFactura != null && tempFactura.ReservacionId != null)
            {
                tempReservacion = await _context.Reservaciones.FirstOrDefaultAsync(r => r.Id == tempFactura.ReservacionId);
                if (tempReservacion != null && tempReservacion.PaqueteId != null)
                {
                    tempPaquete = await _context.Paquetes.FirstOrDefaultAsync(p => p.Id == tempReservacion.PaqueteId);
                }
            }

            string theme = "Su factura de Hotel Beach está disponible";
            string body = $@"
            Estimado/a {tempUsuario.Nombre_Completo}:

            Le informamos que su factura está lista. Puede encontrarla adjunta a este correo.

            Si tiene alguna consulta, no dude en ponerse en contacto con nosotros.

            Gracias por su confianza.

            Atentamente,  
            Hotel Beach  
            hotelbeachnotificaciones@gmail.com | 1234-1234
            ";

            var pdfDocument = invoiceService.GetInvoice(tempFactura, tempUsuario, tempReservacion, tempPaquete, formaPago);
            using var stream = new MemoryStream();
            pdfDocument.Save(stream, false);
            var pdfBytes = stream.ToArray();
            await emailService.SendEmailWithAttachment(email, theme, body, pdfBytes, "Factura-Hotel-Beach.pdf");
            return Ok(new { message = "Email enviado correctamente" });
        }

    }
}




