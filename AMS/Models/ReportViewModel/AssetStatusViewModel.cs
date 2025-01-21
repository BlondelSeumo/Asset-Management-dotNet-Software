
namespace AMS.Models.ReportViewModel
{
    public class AssetStatusViewModel
    {
        public int New { get; set; }
        public int InUse { get; set; }
        public int Available { get; set; }
        public int Damage { get; set; }
        public int Return { get; set; }
        public int Expired { get; set; }
        public int RequiredLicenseUpdate { get; set; }
        public int Miscellaneous { get; set; }
        public int Total { get; set; }
    }
}
