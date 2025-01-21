using System.ComponentModel.DataAnnotations;
using AMS.Models;

namespace AMS.Models.AssetCategorieViewModel
{
    public class AssetCategorieCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator AssetCategorieCRUDViewModel(AssetCategorie _AssetCategorie)
        {
            return new AssetCategorieCRUDViewModel
            {
                Id = _AssetCategorie.Id,
                Name = _AssetCategorie.Name,
                Description = _AssetCategorie.Description,
                CreatedDate = _AssetCategorie.CreatedDate,
                ModifiedDate = _AssetCategorie.ModifiedDate,
                CreatedBy = _AssetCategorie.CreatedBy,
                ModifiedBy = _AssetCategorie.ModifiedBy,
                Cancelled = _AssetCategorie.Cancelled,
            };
        }

        public static implicit operator AssetCategorie(AssetCategorieCRUDViewModel vm)
        {
            return new AssetCategorie
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
