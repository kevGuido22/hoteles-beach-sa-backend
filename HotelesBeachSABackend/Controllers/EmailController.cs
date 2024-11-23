using HotelesBeachSABackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelesBeachSABackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }



        [HttpPost("Send")]
        public async Task<IActionResult> Send(string email, string theme, string body)
        {
            await emailService.SendEmail(email, theme, body);
            return Ok();
        }

    }
}
