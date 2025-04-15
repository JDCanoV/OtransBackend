using System.Net.Mail;
using System.Net;

namespace OtransBackend.Utilities
{
    public class EmailUtility
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Cambiar por tu servidor SMTP
        private readonly string _smtpUsername = "blancasaludeps@gmail.com";  // Cambiar por tu correo
        private readonly string _smtpPassword = "nrprwwellwbvzmik"; // Cambiar por tu contraseña
        private readonly int _smtpPort = 587; // O el puerto adecuado

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            using (var smtpClient = new SmtpClient(_smtpServer))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                smtpClient.Port = _smtpPort;
                smtpClient.EnableSsl = true; // Habilitar SSL si es necesario
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
