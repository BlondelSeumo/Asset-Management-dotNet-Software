using AMS.Models.AccountViewModels;
using AMS.Models.CommonViewModel;
using AMS.Pages;

namespace AMS.Models.DashboardViewModel
{
    public class SharedUIDataViewModel
    {
        public UserProfile UserProfile { get; set; }
        public ApplicationInfo ApplicationInfo { get; set; }
        public MainMenuViewModel MainMenuViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }
    }
}
