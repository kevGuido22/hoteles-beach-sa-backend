using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Net.Http;
using System.Net.Mail;

namespace HotelesBeachSABackend.Services
{
    public interface IEmailService
    {
        Task SendEmail(string emailRecipient, string tema, string cuerpo);
        Task SendEmailWithAttachment(string emailRecipient, string tema, string cuerpo, byte[] attachment, string attachmentName);
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

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailSender, password);

            var message = new MailMessage(emailSender!, emailRecipient, tema, cuerpo);
            await smtpClient.SendMailAsync(message);
        }

        public async Task SendEmailWithAttachment(string emailRecipient, string tema, string cuerpo,byte[] attachment, string attachmentName){
            var emailSender = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:EMAIL");
            var password = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:PASSWORD");
            var host = configuration.GetValue<string>("EMAIL_CONFIGURATIONS:HOST");
            var port = configuration.GetValue<int>("EMAIL_CONFIGURATIONS:PORT");

            var smtpClient = new SmtpClient(host, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(emailSender, password);

            using var message = new MailMessage(emailSender!, emailRecipient, tema, cuerpo)
            {
                IsBodyHtml = true,
            }; 
            using var stream = new MemoryStream(attachment);
            var emailAttachment = new Attachment(stream, attachmentName);
            message.Attachments.Add(emailAttachment);

            await smtpClient.SendMailAsync(message);

        }
    }
}
