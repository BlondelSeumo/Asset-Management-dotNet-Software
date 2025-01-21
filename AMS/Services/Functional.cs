using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using AMS.Data;
using AMS.Models;
using AMS.Models.AssetHistoryViewModel;
using AMS.Helpers;
using AMS.Models.DashboardViewModel;
using System.Security.Claims;
using AMS.Models.CommonViewModel;
using AMS.ConHelper;

namespace AMS.Services
{
    public class Functional : IFunctional
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IRoles _roles;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;
        private readonly ApplicationInfo _applicationInfo;
        private readonly IAccount _iAccount;
        private readonly ICommon _iCommon;
        private readonly SeedData _CommonData = new SeedData();

        public Functional(UserManager<ApplicationUser> userManager,
           ApplicationDbContext context,
           IRoles roles,
           IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions,
           IAccount iAccount,
           ICommon iCommon)
        {
            _userManager = userManager;
            _context = context;
            _roles = roles;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
            _iAccount = iAccount;
            _iCommon = iCommon;
        }

        public async Task SendEmailBySendGridAsync(string apiKey,
            string fromEmail,
            string fromFullName,
            string subject,
            string message,
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromFullName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, email));
            await client.SendEmailAsync(msg);

        }

        public async Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(fromEmail, fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                smtp.UseDefaultCredentials = false;
                var credential = new NetworkCredential
                {
                    UserName = smtpUser,
                    Password = smtpPassword
                };
                smtp.Credentials = credential;
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.EnableSsl = smtpSSL;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);

            }

        }

        public async Task InitAppData()
        {
            var _GetSupplierList = _CommonData.GetSupplierList();
            foreach (var item in _GetSupplierList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Supplier.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetAssetCategorieList = _CommonData.GetAssetCategorieList();
            foreach (var item in _GetAssetCategorieList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.AssetCategorie.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetAssetSubCategorieList = _CommonData.GetAssetSubCategorieList();
            foreach (var item in _GetAssetSubCategorieList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.AssetSubCategorie.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetAssetStatusList = _CommonData.GetAssetStatusList();
            foreach (var item in _GetAssetStatusList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.AssetStatus.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetDepartmentList = _CommonData.GetDepartmentList();
            foreach (var item in _GetDepartmentList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Department.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetSubDepartmentList = _CommonData.GetSubDepartmentList();
            foreach (var item in _GetSubDepartmentList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.SubDepartment.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetDesignationList = _CommonData.GetDesignationList();
            foreach (var item in _GetDesignationList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.Designation.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetAssetRequestList = _CommonData.GetAssetRequestList();
            foreach (var item in _GetAssetRequestList)
            {
                item.RequestDate = DateTime.Now;
                item.ReceiveDate = DateTime.Now.AddDays(7);
                item.RequestDetails = "Your request note";
                item.Comment = "Your comment";


                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.AssetRequest.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetAssetAssetIssueList = _CommonData.GetAssetAssetIssueList();
            foreach (var item in _GetAssetAssetIssueList)
            {
                item.ExpectedFixDate = DateTime.Now.AddDays(7);
                item.ResolvedDate = DateTime.Now.AddDays(7);
                item.IssueDescription = "Your issue details note";
                item.Comment = "Your comment";


                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.AssetIssue.Add(item);
                await _context.SaveChangesAsync();
            }
            var _GetManageRoleList = _CommonData.GetManageRoleList();
            foreach (var item in _GetManageRoleList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.ManageUserRoles.Add(item);
                await _context.SaveChangesAsync();
            }

            var _GetCompanyInfo = _CommonData.GetCompanyInfo();
            _GetCompanyInfo.CreatedDate = DateTime.Now;
            _GetCompanyInfo.ModifiedDate = DateTime.Now;
            _GetCompanyInfo.CreatedBy = "Admin";
            _GetCompanyInfo.ModifiedBy = "Admin";
            _context.CompanyInfo.Add(_GetCompanyInfo);
            _context.SaveChanges();
        }

        public async Task GenerateUserUserRole()
        {
            var _ManageRole = await _context.ManageUserRoles.ToListAsync();
            var _GetRoleList = await _roles.GetRoleList();

            foreach (var role in _ManageRole)
            {
                foreach (var item in _GetRoleList)
                {
                    ManageUserRolesDetails _ManageRoleDetails = new();
                    _ManageRoleDetails.ManageRoleId = role.Id;
                    _ManageRoleDetails.RoleId = item.RoleId;
                    _ManageRoleDetails.RoleName = item.RoleName;

                    if (role.Id == 1)
                    {
                        _ManageRoleDetails.IsAllowed = true;
                    }
                    else if (role.Id == 2 && item.RoleName == "User Profile" || item.RoleName == "Leave MGS" || item.RoleName == "Dashboard")
                    {
                        _ManageRoleDetails.IsAllowed = true;
                    }
                    else
                    {
                        _ManageRoleDetails.IsAllowed = false;
                    }

                    _ManageRoleDetails.CreatedDate = DateTime.Now;
                    _ManageRoleDetails.ModifiedDate = DateTime.Now;
                    _ManageRoleDetails.CreatedBy = "Admin";
                    _ManageRoleDetails.ModifiedBy = "Admin";
                    _context.Add(_ManageRoleDetails);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task CreateAsset()
        {
            try
            {
                var _GetAssetList = _CommonData.GetAssetList();
                foreach (var item in _GetAssetList)
                {
                    item.AssetId = "ID-" + StaticData.RandomDigits(6);
                    item.Barcode = SampleBarcode.Default;
                    item.QRCode = item.AssetId;
                    item.QRCodeImage = SampleQRCode.Default;

                    item.CreatedDate = DateTime.Now;
                    item.ModifiedDate = DateTime.Now;
                    item.CreatedBy = "Admin";
                    item.ModifiedBy = "Admin";
                    _context.Asset.Add(item);
                    _context.SaveChanges();

                    AssetHistoryCRUDViewModel _AssetHistoryCRUDViewModel = new AssetHistoryCRUDViewModel
                    {
                        AssetId = item.Id,
                        AssignEmployeeId = 0,
                        Action = "Asset Created.",
                        UserName = "Admin"
                    };
                    await _iCommon.AddAssetHistory(_AssetHistoryCRUDViewModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                await _roles.GenerateRolesFromPageList();
                ApplicationUser superAdmin = new ApplicationUser();
                superAdmin.Email = _superAdminDefaultOptions.Email;
                superAdmin.UserName = superAdmin.Email;
                superAdmin.EmailConfirmed = true;
                var result = await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);
                if (result.Succeeded)
                {
                    UserProfile profile = new();
                    profile.ApplicationUserId = superAdmin.Id;
                    profile.FirstName = "Super";
                    profile.LastName = "Admin";
                    profile.PhoneNumber = "+8801674411603";
                    profile.Email = superAdmin.Email;
                    profile.Address = "R/A, Dhaka";
                    profile.Country = "Bangladesh";
                    profile.ProfilePicture = "/images/UserIcon/Admin.png";

                    profile.RoleId = 1;
                    profile.IsApprover = 1;
                    profile.EmployeeId = StaticData.RandomDigits(6);
                    profile.DateOfBirth = DateTime.Now.AddYears(-25);
                    profile.Designation = 1;
                    profile.Department = 1;
                    profile.SubDepartment = 1;
                    profile.JoiningDate = DateTime.Now.AddYears(-1);
                    profile.LeavingDate = DateTime.Now;
                    profile.Designation = 2;
                    profile.Department = 2;
                    profile.SubDepartment = 2;

                    profile.CreatedDate = DateTime.Now;
                    profile.ModifiedDate = DateTime.Now;
                    profile.CreatedBy = "Admin";
                    profile.ModifiedBy = "Admin";

                    await _context.UserProfile.AddAsync(profile);
                    await _context.SaveChangesAsync();

                    await _roles.AddToRoles(superAdmin);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task CreateDefaultOtherUser()
        {
            var _GetUserProfileList = _CommonData.GetUserProfileList();

            foreach (var item in _GetUserProfileList)
            {
                item.RoleId = 2;
                item.IsApprover = 1;
                item.EmployeeId = StaticData.RandomDigits(6);
                item.DateOfBirth = DateTime.Now.AddYears(-25);
                item.Designation = 1;
                item.Department = 1;
                item.SubDepartment = 1;
                item.JoiningDate = DateTime.Now.AddYears(-1);
                item.LeavingDate = DateTime.Now;
                var _ApplicationUser = await _iAccount.CreateUserProfile(item, "Admin");
            }
        }
        public async Task<string> UploadFile(List<IFormFile> files, IWebHostEnvironment env, string uploadFolder)
        {
            var result = "";

            var webRoot = env.WebRootPath;
            var uploads = Path.Combine(webRoot, uploadFolder);
            var extension = "";
            var filePath = "";
            var fileName = "";


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    extension = Path.GetExtension(formFile.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    filePath = Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    result = fileName;

                }
            }

            return result;
        }
        public async Task CreateDefaultEmailSettings()
        {
            //SMTP
            var CountSMTPEmailSetting = _context.SMTPEmailSetting.Count();
            if (CountSMTPEmailSetting < 1)
            {
                SMTPEmailSetting _SMTPEmailSetting = new SMTPEmailSetting
                {
                    UserName = "devmlbd@gmail.com",
                    Password = "",
                    Host = "smtp.gmail.com",
                    Port = 587,
                    IsSSL = true,
                    FromEmail = "devmlbd@gmail.com",
                    FromFullName = "Web Admin Notification",
                    IsDefault = true,

                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin",
                };
                _context.Add(_SMTPEmailSetting);
                await _context.SaveChangesAsync();
            }
            //SendGridOptions
            var CountSendGridSetting = _context.SendGridSetting.Count();
            if (CountSendGridSetting < 1)
            {
                SendGridSetting _SendGridOptions = new SendGridSetting
                {
                    SendGridUser = "",
                    SendGridKey = "",
                    FromEmail = "devmlbd@gmail.com",
                    FromFullName = "Web Admin Notification",
                    IsDefault = false,

                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin",
                };
                _context.Add(_SendGridOptions);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<SharedUIDataViewModel> GetSharedUIData(ClaimsPrincipal _ClaimsPrincipal)
        {
            SharedUIDataViewModel _SharedUIDataViewModel = new();
            ApplicationUser _ApplicationUser = await _userManager.GetUserAsync(_ClaimsPrincipal);
            _SharedUIDataViewModel.UserProfile = _context.UserProfile.SingleOrDefault(x => x.ApplicationUserId.Equals(_ApplicationUser.Id));
            _SharedUIDataViewModel.MainMenuViewModel = await _roles.RolebaseMenuLoad(_ApplicationUser);
            _SharedUIDataViewModel.ApplicationInfo = _applicationInfo;
            return _SharedUIDataViewModel;
        }
        public async Task<DefaultIdentityOptions> GetDefaultIdentitySettings()
        {
            return await _context.DefaultIdentityOptions.Where(x => x.Id == 1).SingleOrDefaultAsync();
        }
        public async Task CreateDefaultIdentitySettings()
        {
            if (_context.DefaultIdentityOptions.Count() < 1)
            {
                DefaultIdentityOptions _DefaultIdentityOptions = new DefaultIdentityOptions
                {
                    PasswordRequireDigit = false,
                    PasswordRequiredLength = 3,
                    PasswordRequireNonAlphanumeric = false,
                    PasswordRequireUppercase = false,
                    PasswordRequireLowercase = false,
                    PasswordRequiredUniqueChars = 0,
                    LockoutDefaultLockoutTimeSpanInMinutes = 30,
                    LockoutMaxFailedAccessAttempts = 5,
                    LockoutAllowedForNewUsers = false,
                    UserRequireUniqueEmail = true,
                    SignInRequireConfirmedEmail = false,

                    CookieHttpOnly = true,
                    CookieExpiration = 150,
                    CookieExpireTimeSpan = 120,
                    LoginPath = "/Account/Login",
                    LogoutPath = "/Account/Logout",
                    AccessDeniedPath = "/Account/AccessDenied",
                    SlidingExpiration = true,

                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = "Admin",
                    ModifiedBy = "Admin",
                };
                _context.Add(_DefaultIdentityOptions);
                await _context.SaveChangesAsync();
            }
        }
    }
}
