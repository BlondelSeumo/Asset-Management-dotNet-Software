namespace AMS.Models.AssetViewModel
{
    public class DownloadPurchaseReceiptViewModel
    {
        public Int64 Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] DocByte { get; set; }
    }
}
