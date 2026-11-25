using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace SmartStock.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage)
        {
            var senderEmail = _configuration["EmailSettings:SenderEmail"]
                ?? throw new InvalidOperationException("SenderEmail is missing.");

            var password = _configuration["EmailSettings:Password"]
                ?? throw new InvalidOperationException("Email password is missing.");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SmartStock", senderEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            message.Body = new BodyBuilder
            {
                HtmlBody = htmlMessage
            }.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                "smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(senderEmail, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}