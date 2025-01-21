using System.ComponentModel.DataAnnotations;
using AMS.Helpers;
using AMS.Models.ManageUserRolesVM;

namespace AMS.Models.UserProfileViewModel
{
    public class UserProfileCRUDViewModel : EntityBase
    {
        [Display(Name = "User Profile Id")]
        public Int64 UserProfileId { get; set; }
        [Display(Name = "Employee Id")]
        public string EmployeeId { get; set; } = "EMP-" + StaticData.RandomDigits(6);
        public string ApplicationUserId { get; set; }
        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; } = DateTime.Today;
        public int Designation { get; set; }
        public string DesignationDisplay { get; set; }
        public int Department { get; set; }
        public string DepartmentDisplay { get; set; }
        [Display(Name = "Sub Department")]
        public int SubDepartment { get; set; }
        public string SubDepartmentDisplay { get; set; }
        [Display(Name = "Joining Date")]
        public DateTime JoiningDate { get; set; } = DateTime.Today;
        [Display(Name = "Leaving Date")]
        public DateTime LeavingDate { get; set; } = DateTime.Today;
        public string PhoneNumber { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Display(Name = "Password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [Display(Name = "Confirm Password")]
        [Required]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePictureDetails { get; set; }
        public string ProfilePicture { get; set; } = "/upload/blank-person.png";
        [Display(Name = "User Role")]
        public Int64 RoleId { get; set; }
        public string RoleIdDisplay { get; set; }
        public int IsApprover { get; set; }
        public string IsApproverDisplay { get; set; }
        public string CurrentURL { get; set; }
        public List<AssetAssignedCRUDViewModel> listAssetAssignedCRUDViewModel { get; set; }


        public static implicit operator UserProfileCRUDViewModel(UserProfile vm)
        {
            return new UserProfileCRUDViewModel
            {
                UserProfileId = vm.UserProfileId,
                ApplicationUserId = vm.ApplicationUserId,
                EmployeeId = vm.EmployeeId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DateOfBirth = vm.DateOfBirth,
                Designation = vm.Designation,
                Department = vm.Department,
                SubDepartment = vm.SubDepartment,
                JoiningDate = vm.JoiningDate,
                LeavingDate = vm.LeavingDate,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,
                ProfilePicture = vm.ProfilePicture,
                RoleId = vm.RoleId,
                IsApprover = vm.IsApprover,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }

        public static implicit operator UserProfile(UserProfileCRUDViewModel vm)
        {
            return new UserProfile
            {
                UserProfileId = vm.UserProfileId,
                ApplicationUserId = vm.ApplicationUserId,
                EmployeeId = vm.EmployeeId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DateOfBirth = vm.DateOfBirth,
                Designation = vm.Designation,
                Department = vm.Department,
                SubDepartment = vm.SubDepartment,
                JoiningDate = vm.JoiningDate,
                LeavingDate = vm.LeavingDate,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Address = vm.Address,
                Country = vm.Country,
                ProfilePicture = vm.ProfilePicture,
                RoleId = vm.RoleId,
                IsApprover = vm.IsApprover,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
