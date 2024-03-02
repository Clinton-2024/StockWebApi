using StockWebApi.Core.Dtos.Mail;

namespace StockWebApi.Interfaces
{
    public interface IMail_Service
    {
        Task SendEmailAsync(MailRequestDto mailRequest);
    }
}
