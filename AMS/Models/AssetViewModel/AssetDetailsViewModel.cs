using AMS.Models.UserProfileViewModel;

namespace AMS.Models.AssetViewModel
{
    public class AssetDetailsViewModel : EntityBase
    {
        public AssetCRUDViewModel AssetCRUDViewModel { get; set; }
        public UserProfileCRUDViewModel UserProfileCRUDViewModel { get; set; }       
    }
}
