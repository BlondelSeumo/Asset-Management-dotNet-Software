using System;

namespace AMS.Models.AssetSubCategorieViewModel
{
    public class AssetSubCategorieGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 AssetCategorieId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
