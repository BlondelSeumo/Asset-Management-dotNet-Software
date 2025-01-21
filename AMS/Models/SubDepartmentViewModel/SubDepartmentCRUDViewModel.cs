using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models.SubDepartmentViewModel
{
    public class SubDepartmentCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Department")]
        public Int64 DepartmentId { get; set; }
        public string DepartmentDisplay { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator SubDepartmentCRUDViewModel(SubDepartment _SubDepartment)
        {
            return new SubDepartmentCRUDViewModel
            {
                Id = _SubDepartment.Id,
                DepartmentId = _SubDepartment.DepartmentId,
                Name = _SubDepartment.Name,
                Description = _SubDepartment.Description,
                CreatedDate = _SubDepartment.CreatedDate,
                ModifiedDate = _SubDepartment.ModifiedDate,
                CreatedBy = _SubDepartment.CreatedBy,
                ModifiedBy = _SubDepartment.ModifiedBy,
                Cancelled = _SubDepartment.Cancelled,
            };
        }

        public static implicit operator SubDepartment(SubDepartmentCRUDViewModel vm)
        {
            return new SubDepartment
            {
                Id = vm.Id,
                DepartmentId = vm.DepartmentId,
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
