using AMS.Data;
using AMS.Models;
using AMS.Models.CommonViewModel;
using AMS.Models.ManageUserRolesVM;
using AMS.Pages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace AMS.Services
{
    public class Roles : IRoles
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public Roles(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task GenerateRolesFromPageList()
        {
            Type _Type = typeof(RoleViewModel);
            foreach (var item in _Type.GetFields())
            {
                string _RoleName = (string)item.GetValue(item);
                if (!await _roleManager.RoleExistsAsync(_RoleName))
                    await _roleManager.CreateAsync(new IdentityRole(_RoleName));
            }
        }

        public async Task<string> CreateSingleRole(string _RoleName)
        {
            if (!await _roleManager.RoleExistsAsync(_RoleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(_RoleName));
                return "Role has been Created. Role Name: " + _RoleName;
            }
            else
            {
                return "Role Already Exits. Role Name: " + _RoleName;
            }
        }
        public async Task AddToRoles(ApplicationUser _ApplicationUser)
        {
            if (_ApplicationUser != null)
            {
                var roles = _roleManager.Roles;
                List<string> listRoles = new List<string>();
                foreach (var item in roles)
                {
                    listRoles.Add(item.Name);
                }
                await _userManager.AddToRolesAsync(_ApplicationUser, listRoles);
            }
        }
        public async Task<MainMenuViewModel> RolebaseMenuLoad(ApplicationUser _ApplicationUser)
        {
            MainMenuViewModel _MainMenuViewModel = new();
            var _PropertyInfo = typeof(MainMenuViewModel).GetProperties();
            var _Roles = await _roleManager.Roles.OrderBy(x => x.Name).ToListAsync();
            try
            {
                foreach (var role in _Roles)
                {
                    var _PropertyName = _PropertyInfo.Where(x => x.Name == Regex.Replace(role.Name, @"\s+", "")).SingleOrDefault();
                    if (_PropertyName != null)
                    {
                        var _IsInRoleAsync = await _userManager.IsInRoleAsync(_ApplicationUser, role.Name);
                        if (_IsInRoleAsync)
                            _PropertyName.SetValue(_MainMenuViewModel, true);
                        else
                            _PropertyName.SetValue(_MainMenuViewModel, false);
                    }
                }
                return _MainMenuViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<MainMenuViewModel> ManageUserRolesDetailsByUser(ApplicationUser _ApplicationUser, ApplicationDbContext _context)
        {
            MainMenuViewModel _MainMenuViewModel = new();
            var _UserProfile = await _context.UserProfile.Where(x => x.ApplicationUserId == _ApplicationUser.Id).SingleOrDefaultAsync();
            var _ManageRoleDetails = await _context.ManageUserRolesDetails
            .Where(x => x.ManageRoleId == _UserProfile.RoleId).OrderBy(x => x.RoleName).ToArrayAsync();
            var _PropertyInfo = typeof(MainMenuViewModel).GetProperties().OrderBy(x => x.Name).ToArray();

            for (int i = 0; i < _PropertyInfo.Count(); i++)
            {
                _PropertyInfo[i].SetValue(_MainMenuViewModel, _ManageRoleDetails[i].IsAllowed);
            }
            return _MainMenuViewModel;
        }
        public async Task<List<ManageUserRolesDetails>> GetRolesByUser(GetRolesByUserViewModel vm)
        {
            List<ManageUserRolesDetails> list = new();
            var user = await vm.UserManager.FindByIdAsync(vm.ApplicationUserId);
            if (user != null)
            {
                foreach (var role in vm.listIdentityRole)
                {
                    var _ManageUserRolesDetails = new ManageUserRolesDetails
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };
                    var _IsInRoleAsync = await vm.UserManager.IsInRoleAsync(user, role.Name);
                    if (_IsInRoleAsync)
                        _ManageUserRolesDetails.IsAllowed = true;
                    else
                        _ManageUserRolesDetails.IsAllowed = false;
                    list.Add(_ManageUserRolesDetails);
                }
            }

            list = list.OrderBy(x => x.RoleName).ToList();
            return list;
        }

        public async Task<List<ManageUserRolesDetails>> GetRoleList()
        {
            List<ManageUserRolesDetails> list = new List<ManageUserRolesDetails>();
            var _Roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in _Roles)
            {
                var _ManageUserRolesDetails = new ManageUserRolesDetails
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsAllowed = false
                };
                list.Add(_ManageUserRolesDetails);
            }
            return list.OrderBy(x => x.RoleName).ToList();
        }
        public async Task<JsonResultViewModel> UpdateUserRoles(ManageUserRolesCRUDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var _ApplicationUser = await _userManager.FindByIdAsync(vm.ApplicationUserId);
                if (_ApplicationUser == null)
                {
                    _JsonResultViewModel.IsSuccess = false;
                    _JsonResultViewModel.AlertMessage = "User not found";
                    return _JsonResultViewModel;
                }
                var roles = await _userManager.GetRolesAsync(_ApplicationUser);
                var result = await _userManager.RemoveFromRolesAsync(_ApplicationUser, roles);
                if (!result.Succeeded)
                {
                    _JsonResultViewModel.IsSuccess = false;
                    _JsonResultViewModel.AlertMessage = "Cannot remove user existing roles";
                    return _JsonResultViewModel;
                }
                result = await _userManager.AddToRolesAsync(_ApplicationUser, vm.listManageUserRolesDetails.Where(x => x.IsAllowed).Select(y => y.RoleName));
                if (!result.Succeeded)
                {
                    _JsonResultViewModel.IsSuccess = false;
                    _JsonResultViewModel.AlertMessage = "Cannot add selected roles to user";
                    return _JsonResultViewModel;
                }
                _JsonResultViewModel.AlertMessage = "Role update Successfully. User Name: " + _ApplicationUser.Email;
                _JsonResultViewModel.IsSuccess = true;
                return _JsonResultViewModel;
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return _JsonResultViewModel;
                throw;
            }
        }
    }
}
