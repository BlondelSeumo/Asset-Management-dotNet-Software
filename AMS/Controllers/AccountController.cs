using AMS.Data;
using AMS.Helpers;
using AMS.Models;
using AMS.Models.AccountViewModels;
using AMS.Models.CommonViewModel;
using AMS.Models.UserProfileViewModel;
using AMS.Services;
using LeaveMGS.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UAParser;

namespace AMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly ICommon _iCommon;
        private readonly IRoles _roles;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ICommon iCommon,
            IRoles roles,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
            _iCommon = iCommon;
            _roles = roles;
            _configuration = configuration;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                await InserLoginHistory(true, result.Succeeded, model);

                string _AlertMessage = "User logged in.";
                if (result.Succeeded)
                {
                    //await JWTHandle(model);
                    HttpContext.Session.SetString("LoginUserName", model.Email);
                    _logger.LogInformation(_AlertMessage);

                    _JsonResultViewModel.AlertMessage = _AlertMessage;
                    _JsonResultViewModel.IsSuccess = result.Succeeded;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    _AlertMessage = "Invalid login attempt.";
                    _logger.LogInformation(_AlertMessage);
                    _JsonResultViewModel.AlertMessage = _AlertMessage;
                    _JsonResultViewModel.IsSuccess = result.Succeeded;
                    return new JsonResult(_JsonResultViewModel);
                }
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }

        private async Task JWTHandle(LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = AddJWTOptions.GetToken(authClaims, _configuration);

                /*
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
                */
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveUserInfoFromBrowser(UserInfoFromBrowser _UserInfoFromBrowser)
        {
            _UserInfoFromBrowser.CreatedDate = DateTime.Now;
            _UserInfoFromBrowser.ModifiedDate = DateTime.Now;
            _context.Add(_UserInfoFromBrowser);
            var result = await _context.SaveChangesAsync();
            return new JsonResult(_UserInfoFromBrowser);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWith2fa(bool rememberMe, string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var model = new LoginWith2faViewModel { RememberMe = rememberMe };
            ViewData["ReturnUrl"] = returnUrl;

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2fa(LoginWith2faViewModel model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with 2fa.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID {UserId}.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginWithRecoveryCode(string returnUrl = null)
        {
            // Ensure the user has gone through the username & password screen first
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new ApplicationException($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            JsonResultViewModel _JsonResultViewModel = new JsonResultViewModel();
            try
            {
                var _ApplicationUser = new ApplicationUser
                {
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    Email = model.Email,
                    EmailConfirmed = false
                };
                var result = await _userManager.CreateAsync(_ApplicationUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(_ApplicationUser, "Dashboard");
                    await _userManager.AddToRoleAsync(_ApplicationUser, "User Profile");

                    //Insert: UserProfile
                    UserProfile _UserProfile = new UserProfile
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Email = model.Email,
                        Address = model.Address,
                        Country = model.Country,
                        ApplicationUserId = _ApplicationUser.Id,

                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedBy = HttpContext.User.Identity.Name,
                        ModifiedBy = HttpContext.User.Identity.Name
                    };
                    var result2 = await _context.UserProfile.AddAsync(_UserProfile);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("User created a new account with password.");

                    var _DefaultIdentityOptions = await _context.DefaultIdentityOptions.FirstOrDefaultAsync(m => m.Id == 1);
                    if (_DefaultIdentityOptions.SignInRequireConfirmedEmail)
                    {
                        var _ConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(_ApplicationUser);
                        var callbackUrl = Url.EmailConfirmationLink(_ApplicationUser.Id, _ConfirmationToken, Request.Scheme);
                        await _emailSender.SendEmailConfirmationAsync(_ApplicationUser.Email, callbackUrl);
                    }
                    await _signInManager.SignInAsync(_ApplicationUser, isPersistent: false);
                    LoginViewModel _LoginViewModel = new LoginViewModel
                    {
                        Email = model.Email,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude
                    };
                    await InserLoginHistory(true, true, _LoginViewModel);
                    _logger.LogInformation("User created a new account with password.");
                    _JsonResultViewModel.AlertMessage = "User Created Successfully. User Name: " + _ApplicationUser.Email;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    string errorMessage = string.Empty;
                    foreach (var item in result.Errors)
                    {
                        errorMessage = errorMessage + item.Description;
                    }
                    _JsonResultViewModel.AlertMessage = "User Creation Failed." + errorMessage;
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LoginViewModel vm)
        {
            await _signInManager.SignOutAsync();
            await InserLoginHistory(false, true, vm);
            _logger.LogInformation("User logged out.");
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToAction(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction(nameof(Lockout));
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLogin", new ExternalLoginViewModel { Email = email });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    throw new ApplicationException("Error loading external login information during confirmation.");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(nameof(ExternalLogin), model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userId}'.");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                JsonResultViewModel _JsonResultViewModel = new();
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    //return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    _JsonResultViewModel.AlertMessage = "User not found with this email: " + model.Email;
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
                else if (!(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    _JsonResultViewModel.AlertMessage = "User email is not confirmed yet. Please confirm email first. Email: " + model.Email;
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);

                await _emailSender.SendEmailForgotPasswordAsync(model.Email, callbackUrl);

                _JsonResultViewModel.AlertMessage = "Success, . Email: " + model.Email;
                _JsonResultViewModel.IsSuccess = true;
                return new JsonResult(_JsonResultViewModel);
            }
            return new JsonResult(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("A code must be supplied for password reset.");
            }
            var model = new ResetPasswordViewModel { Code = code };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    _JsonResultViewModel.AlertMessage = "Incorrect information.";
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.NewPassword);
                if (result.Succeeded)
                {
                    _JsonResultViewModel.AlertMessage = "Success. Email: " + model.Email;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    _JsonResultViewModel.AlertMessage = "Reset Password Failed. Email: " + model.Email;
                    _JsonResultViewModel.IsSuccess = false;
                    return new JsonResult(_JsonResultViewModel);
                }
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                return new JsonResult(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private async Task InserLoginHistory(bool IsLoginAction, bool _IsSuccess, LoginViewModel _LoginViewModel)
        {
            var _Headers = HttpContext.Request.Headers["User-Agent"];
            var _Parser = Parser.GetDefault();
            ClientInfo _ClientInfo = _Parser.Parse(_Headers);

            LoginHistory vm = new LoginHistory
            {
                Browser = _ClientInfo.UA.Family,
                OperatingSystem = _ClientInfo.OS.Family,
                Device = _ClientInfo.Device.Family,
                ActionStatus = _IsSuccess == true ? "Success" : "Failed",
                Latitude = _LoginViewModel.Latitude,
                Longitude = _LoginViewModel.Longitude,
            };

            if (IsLoginAction)
            {
                vm.UserName = _LoginViewModel.Email;
                vm.LoginTime = DateTime.Now;
                vm.Duration = 0;
                vm.Action = "Login";

                vm.CreatedBy = _LoginViewModel.Email;
                vm.ModifiedBy = _LoginViewModel.Email;
            }
            else
            {
                string _UserName = HttpContext.User.Identity.Name;
                var _LoginHistory = _context.LoginHistory.Where(x => x.UserName == _UserName && x.Action == "Login").OrderByDescending(x => x.CreatedDate).Take(1).SingleOrDefault();
                vm.UserName = _UserName;
                vm.LoginTime = _LoginHistory.LoginTime; //Bug null
                vm.LogoutTime = DateTime.Now;
                vm.Duration = (DateTime.Now - _LoginHistory.LoginTime).TotalMinutes;
                vm.Action = "Logout";

                vm.CreatedBy = _UserName;
                vm.ModifiedBy = _UserName;
            }
            await _iCommon.InsertLoginHistory(vm, _ClientInfo);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }
        }

        #endregion
    }
}