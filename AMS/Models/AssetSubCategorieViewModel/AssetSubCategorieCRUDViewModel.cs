using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models.AssetSubCategorieViewModel
{
    public class AssetSubCategorieCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Asset Categorie")]
        public Int64 AssetCategorieId { get; set; }
        public string AssetCategorieDisplay { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator AssetSubCategorieCRUDViewModel(AssetSubCategorie _AssetSubCategorie)
        {
            return new AssetSubCategorieCRUDViewModel
            {
                Id = _AssetSubCategorie.Id,
                AssetCategorieId = _AssetSubCategorie.AssetCategorieId,
                Name = _AssetSubCategorie.Name,
                Description = _AssetSubCategorie.Description,
                CreatedDate = _AssetSubCategorie.CreatedDate,
                ModifiedDate = _AssetSubCategorie.ModifiedDate,
                CreatedBy = _AssetSubCategorie.CreatedBy,
                ModifiedBy = _AssetSubCategorie.ModifiedBy,
                Cancelled = _AssetSubCategorie.Cancelled,
            };
        }

        public static implicit operator AssetSubCategorie(AssetSubCategorieCRUDViewModel vm)
        {
            return new AssetSubCategorie
            {
                Id = vm.Id,
                AssetCategorieId = vm.AssetCategorieId,
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
