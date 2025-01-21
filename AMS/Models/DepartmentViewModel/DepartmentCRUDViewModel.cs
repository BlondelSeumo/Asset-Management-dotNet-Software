using System.ComponentModel.DataAnnotations;
using AMS.Models;

namespace AMS.Models.DepartmentViewModel
{
    public class DepartmentCRUDViewModel : EntityBase
    {
        [Display(Name = "SL"), Required]
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }


        public static implicit operator DepartmentCRUDViewModel(Department _Department)
        {
            return new DepartmentCRUDViewModel
            {
                Id = _Department.Id,
                Name = _Department.Name,
                Description = _Department.Description,
                CreatedDate = _Department.CreatedDate,
                ModifiedDate = _Department.ModifiedDate,
                CreatedBy = _Department.CreatedBy,
                ModifiedBy = _Department.ModifiedBy,
                Cancelled = _Department.Cancelled,
            };
        }

        public static implicit operator Department(DepartmentCRUDViewModel vm)
        {
            return new Department
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
