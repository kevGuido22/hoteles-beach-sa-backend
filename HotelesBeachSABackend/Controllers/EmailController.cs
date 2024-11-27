using HotelesBeachSABackend.Models;
using HotelesBeachSABackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        private readonly InvoiceService invoiceService;
        public EmailController(IEmailService emailService, InvoiceService invoiceService)
        {
            this.emailService = emailService;
            this.invoiceService = invoiceService;
        }

        //[HttpPost("Send")]
        //public async Task<IActionResult> Send(string email, string theme, string body)
        //{
        //    await emailService.SendEmail(email, theme, body);
        //    return Ok();
        //}

        [HttpPost("SendWithInvoice")]
        public async Task<IActionResult> SendWithInvoice(string email, string nombreUsuario, int idFactura)
        {
            string theme = "Su factura de Hotel Beach está disponible";
            string body = $@"
            Estimado/a {nombreUsuario}:

            Le informamos que su factura está lista. Puede encontrarla adjunta a este correo.

            Si tiene alguna consulta, no dude en ponerse en contacto con nosotros.

            Gracias por su confianza.

            Atentamente,  
            Hotel Beach  
            hotelbeachnotificaciones@gmail.com | 1234-1234
            ";

            var pdfDocument = invoiceService.GetInvoice(idFactura);
            using var stream = new MemoryStream();
            pdfDocument.Save(stream, false);
            var pdfBytes = stream.ToArray();
            await emailService.SendEmailWithAttachment(email, theme, body, pdfBytes, "Factura-Hotel-Beach.pdf");
            return Ok(new { message = "Email enviado correctamente" });
        }

    }
}




