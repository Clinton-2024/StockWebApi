using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using StockWebApi.Core.Dtos.Mail;
using StockWebApi.Interfaces;

namespace StockWebApi.Services
{
    public class Mail_Service : IMail_Service
    {
        private readonly IConfiguration _configuration;


        public Mail_Service( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(MailRequestDto mailRequest)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_configuration["MailSettings:Displayname"], _configuration["MailSettings:From"]));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();

            if (mailRequest.Attachments != null)
            {
                byte[] filebytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            filebytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.Name, filebytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_configuration["MailSettings:Host"], int.Parse( _configuration["MailSettings:Port"]) , SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["MailSettings:Username"], _configuration["MailSettings:Password"]);

            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}
