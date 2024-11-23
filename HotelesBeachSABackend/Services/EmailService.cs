using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Mail;

namespace HotelesBeachSABackend.Services
{
    public interface IEmailService
    {
        Task SendEmail(string emailRecipient, string tema, string cuerpo);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;
        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task SendEmail(string emailRecipient, string tema, string cuerpo)
        {
            var emailSender = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:EMAIL");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:PASSWORD");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATIONS:PORT");

            var smptClient = new SmtpClient(host, port);
            smptClient.EnableSsl = true;
            smptClient.UseDefaultCredentials = false;
            smptClient.Credentials = new NetworkCredential(emailSender, password);

            var message = new MailMessage(emailSender!, emailRecipient, tema, cuerpo);
            await smptClient.SendMailAsync(message);
        }

    }
}
