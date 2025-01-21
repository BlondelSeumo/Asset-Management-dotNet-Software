using System.ComponentModel.DataAnnotations;
using AMS.Models;

namespace AMS.Models.AssetStatusViewModel
{
    public class AssetStatusCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator AssetStatusCRUDViewModel(AssetStatus _AssetStatus)
        {
            return new AssetStatusCRUDViewModel
            {
                Id = _AssetStatus.Id,
                Name = _AssetStatus.Name,
                Description = _AssetStatus.Description,
                CreatedDate = _AssetStatus.CreatedDate,
                ModifiedDate = _AssetStatus.ModifiedDate,
                CreatedBy = _AssetStatus.CreatedBy,
                ModifiedBy = _AssetStatus.ModifiedBy,
                Cancelled = _AssetStatus.Cancelled,
            };
        }

        public static implicit operator AssetStatus(AssetStatusCRUDViewModel vm)
        {
            return new AssetStatus
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
