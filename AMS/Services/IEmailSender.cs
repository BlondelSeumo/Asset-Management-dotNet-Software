using System.Threading.Tasks;

namespace AMS.Services
{
    public interface IEmailSender
    {
        Task<Task> SendEmailAsync(string email, string subject, string message);
    }
}
