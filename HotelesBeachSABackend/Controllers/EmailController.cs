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
        //inicio variables para crear el PDF
        


        //fin variables para crear el PDF


        public EmailController(IEmailService emailService, InvoiceService invoiceService)
        {
            this.emailService = emailService;
            this.invoiceService = invoiceService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send(string email, string theme, string body)
        {
            await emailService.SendEmail(email, theme, body);
            return Ok();
        }

        [HttpPost("SendWithInvoice")]
        public async Task<IActionResult> SendWithInvoice(string email, string theme, string body)
        {
            var pdfDocument = invoiceService.GetInvoice();
            using var stream = new MemoryStream();
            pdfDocument.Save(stream, false);
            var pdfBytes = stream.ToArray();
            await emailService.SendEmailWithAttachment(email, theme, body, pdfBytes, "Invoice.pdf");
            return Ok(new {message = "Email enviado correctamente"});
        }

    }
}




