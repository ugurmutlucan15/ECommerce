using CartService.Entities;
using CartService.Services.Interfaces;
using CartService.Settings.Interfaces;

using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CartService.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailSettings _settings;

        public EmailSender(IEmailSettings settings)
        {
            _settings = settings;
        }

        public Task SendMail(string email, IEnumerable<Cart> carts)
        {
            if (_settings.Stmp == "")
            {
                return Task.CompletedTask;
            }

            SmtpClient smtpClient = new SmtpClient(_settings.Stmp, _settings.Port);

            smtpClient.Credentials = new System.Net.NetworkCredential(_settings.UserName, _settings.Password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = _settings.SSL;
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("info@MyWebsiteDomainName", "MyWeb Site");
            mail.To.Add(new MailAddress(email));
            mail.Subject = "Checkout";
            mail.Body = "Siparişiniz Oluşturuldu.";

            smtpClient.Send(mail);

            return Task.CompletedTask;
        }
    }
}