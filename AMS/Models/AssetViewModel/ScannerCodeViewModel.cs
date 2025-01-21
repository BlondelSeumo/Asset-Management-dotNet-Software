
namespace AMS.Models.AssetViewModel
{
    public class ScannerCodeViewModel
    {
        public Int64 Id { get; set; }
        public string AssetName { get; set; }
        public string Barcode { get; set; }
        public List<ScannerCodeViewModel> listScannerCodeViewModel { get; set; }
        public List<ScannerCodeViewModel> sublistScannerCodeViewModel { get; set; }
    }
}
