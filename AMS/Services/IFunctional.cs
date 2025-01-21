using System.Security.Claims;
using AMS.Models;
using AMS.Models.DashboardViewModel;

namespace AMS.Services
{
    public interface IFunctional
    {
        Task InitAppData();
        Task GenerateUserUserRole();
        Task CreateAsset();
        Task CreateDefaultSuperAdmin();
        Task CreateDefaultOtherUser();

        Task SendEmailBySendGridAsync(string apiKey,
            string fromEmail,
            string fromFullName,
            string subject,
            string message,
            string email);

        Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL);

        Task<SharedUIDataViewModel> GetSharedUIData(ClaimsPrincipal _ClaimsPrincipal);
        Task CreateDefaultEmailSettings();

        Task CreateDefaultIdentitySettings();
        Task<DefaultIdentityOptions> GetDefaultIdentitySettings();
    }
}
