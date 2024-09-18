using CityInfo.API.Models;

namespace CityInfo.API.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailRequest emailRequest);
    }
}
