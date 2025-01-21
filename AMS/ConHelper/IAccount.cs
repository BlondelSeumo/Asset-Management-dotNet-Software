using AMS.Models;
using AMS.Models.AccountViewModels;
using AMS.Models.UserProfileViewModel;
using Microsoft.AspNetCore.Identity;

namespace AMS.ConHelper
{
    public interface IAccount
    {
        Task<Tuple<ApplicationUser, IdentityResult>> CreateUserAccount(CreateUserAccountViewModel _CreateUserAccountViewModel);
        Task<Tuple<ApplicationUser, string>> CreateUserProfile(UserProfileCRUDViewModel vm, string LoginUser);     
    }
}
