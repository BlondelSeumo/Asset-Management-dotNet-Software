using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models.DesignationViewModel
{
    public class DesignationCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }



        public static implicit operator DesignationCRUDViewModel(Designation _Designation)
        {
            return new DesignationCRUDViewModel
            {
                Id = _Designation.Id,
                Name = _Designation.Name,
                Description = _Designation.Description,
                CreatedDate = _Designation.CreatedDate,
                ModifiedDate = _Designation.ModifiedDate,
                CreatedBy = _Designation.CreatedBy,
                ModifiedBy = _Designation.ModifiedBy,
                Cancelled = _Designation.Cancelled,

            };
        }

        public static implicit operator Designation(DesignationCRUDViewModel vm)
        {
            return new Designation
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
