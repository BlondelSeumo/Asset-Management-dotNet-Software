using System;

namespace AMS.Models
{
    public class Asset : EntityBase
    {
        public Int64 Id { get; set; }
        public string AssetId { get; set; }
        public string AssetModelNo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public int SubCategory { get; set; }
        public int? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public int Supplier { get; set; }
        public string Location { get; set; }
        public int Department { get; set; }
        public int SubDepartment { get; set; }
        public int? WarranetyInMonth { get; set; }
        public int? DepreciationInMonth { get; set; }
        public string ImageURL { get; set; }
        public string PurchaseReceipt { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public DateTime DateOfManufacture { get; set; }
        public DateTime YearOfValuation { get; set; }
        public Int64 AssignEmployeeId { get; set; }
        public int AssetStatus { get; set; }
        public bool IsAvilable { get; set; }
        public string Note { get; set; }
        public string Barcode { get; set; }
        public string QRCode { get; set; }
        public string QRCodeImage { get; set; }
    }
}
