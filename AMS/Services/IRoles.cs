using AMS.Data;
using AMS.Models;
using AMS.Models.CommonViewModel;
using AMS.Models.ManageUserRolesVM;
using AMS.Pages;

namespace AMS.Services
{
    public interface IRoles
    {
        Task GenerateRolesFromPageList();
        Task<string> CreateSingleRole(string _RoleName);
        Task AddToRoles(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> RolebaseMenuLoad(ApplicationUser _ApplicationUser);
        Task<MainMenuViewModel> ManageUserRolesDetailsByUser(ApplicationUser _ApplicationUser, ApplicationDbContext _context);
        Task<List<ManageUserRolesDetails>> GetRolesByUser(GetRolesByUserViewModel vm);
        Task<List<ManageUserRolesDetails>> GetRoleList();
        Task<JsonResultViewModel> UpdateUserRoles(ManageUserRolesCRUDViewModel vm);
    }
}
