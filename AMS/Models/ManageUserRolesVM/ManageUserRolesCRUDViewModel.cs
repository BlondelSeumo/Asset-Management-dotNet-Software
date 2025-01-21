using System.ComponentModel.DataAnnotations;

namespace AMS.Models.ManageUserRolesVM
{
    public class ManageUserRolesCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Display(Name = "Name"), Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string ApplicationUserId { get; set; } 
        public List<ManageUserRolesDetails> listManageUserRolesDetails { get; set; } //ManageUserRolesDetails ManageUserRolesViewModel 27

        public static implicit operator ManageUserRolesCRUDViewModel(ManageUserRoles vm)
        {
            return new ManageUserRolesCRUDViewModel
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }

        public static implicit operator ManageUserRoles(ManageUserRolesCRUDViewModel vm)
        {
            return new ManageUserRoles
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
