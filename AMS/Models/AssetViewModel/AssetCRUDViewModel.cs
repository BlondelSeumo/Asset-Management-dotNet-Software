using AMS.Helpers;
using AMS.Models.AssetHistoryViewModel;
using AMS.Models.AssetIssueViewModel;
using AMS.Models.AssetRequestViewModel;
using AMS.Models.CommentViewModel;
using AMS.Models.CompanyInfoViewModel;
using AMS.Models.UserProfileViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models.AssetViewModel
{
    public class AssetCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Asset Id")]
        [Required]
        public string AssetId { get; set; }
        [Display(Name = "Asset Model No")]
        public string AssetModelNo { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public string CategoryDisplay { get; set; }
        [Display(Name = "Sub Category")]
        public int SubCategory { get; set; }
        public string SubCategoryDisplay { get; set; }
        public int? Quantity { get; set; }
        [Display(Name = "Unit Price")]
        public double? UnitPrice { get; set; }
        public int Supplier { get; set; }
        public string SupplierDisplay { get; set; }
        public string Location { get; set; }
        public int Department { get; set; }
        public string DepartmentDisplay { get; set; }
        [Display(Name = "Sub Department")]
        public int SubDepartment { get; set; }
        public string SubDepartmentDisplay { get; set; }
        [Display(Name = "Warranety In Month")]
        public int? WarranetyInMonth { get; set; }
        [Display(Name = "Depreciation In Month")]
        public int? DepreciationInMonth { get; set; }
        [Display(Name = "Image")]
        public string ImageURL { get; set; } = "/upload/blank-asset.png";
        public IFormFile ImageURLDetails { get; set; }
        [Display(Name = "Purchase Receipt")]
        public string PurchaseReceipt { get; set; } = "/upload/blank_purchasereceipt.pdf";
        public IFormFile PurchaseReceiptDetails { get; set; }
        [Display(Name = "Date Of Purchase")]
        public DateTime DateOfPurchase { get; set; } = DateTime.Now;
        [Display(Name = "Date Of Manufacture")]
        public DateTime DateOfManufacture { get; set; } = DateTime.Now;
        [Display(Name = "Year Of Valuation")]
        public DateTime YearOfValuation { get; set; } = DateTime.Now;
        [Display(Name = "Assign Employee")]
        public Int64 AssignEmployeeId { get; set; }
        public string AssignEmployeeDisplay { get; set; }
        [Display(Name = "Asset Status")]
        public int AssetStatus { get; set; } = AssetStatusValue.New;
        public string AssetStatusDisplay { get; set; }
        [Display(Name = "Is Avilable")]
        public bool IsAvilable { get; set; }
        public string Note { get; set; }
        public string Barcode { get; set; }
        public string QRCode { get; set; }
        public string QRCodeImage { get; set; }
        [Display(Name = "Comment Message")]
        public string CommentMessage { get; set; }
        public string CurrentURL { get; set; }
        public bool IsAdmin { get; set; }
        public UserProfileCRUDViewModel UserProfileCRUDViewModel { get; set; }
        public List<AssetHistoryCRUDViewModel> listAssetHistoryCRUDViewModel { get; set; }
        public List<CommentCRUDViewModel> listCommentCRUDViewModel { get; set; }
        public List<AssetRequestCRUDViewModel> listAssetRequestCRUDViewModel { get; set; }
        public List<AssetIssueCRUDViewModel> listAssetIssueCRUDViewModel { get; set; }
        public CompanyInfoCRUDViewModel CompanyInfoCRUDViewModel { get; set; }

        public static implicit operator AssetCRUDViewModel(Asset _Asset)
        {
            return new AssetCRUDViewModel
            {
                Id = _Asset.Id,
                AssetId = _Asset.AssetId,
                AssetModelNo = _Asset.AssetModelNo,
                Name = _Asset.Name,
                Description = _Asset.Description,
                Category = _Asset.Category,
                SubCategory = _Asset.SubCategory,
                Quantity = _Asset.Quantity,
                UnitPrice = _Asset.UnitPrice,
                Supplier = _Asset.Supplier,
                Location = _Asset.Location,
                Department = _Asset.Department,
                SubDepartment = _Asset.SubDepartment,
                WarranetyInMonth = _Asset.WarranetyInMonth,
                DepreciationInMonth = _Asset.DepreciationInMonth,
                ImageURL = _Asset.ImageURL,
                PurchaseReceipt = _Asset.PurchaseReceipt,
                DateOfPurchase = _Asset.DateOfPurchase,
                DateOfManufacture = _Asset.DateOfManufacture,
                YearOfValuation = _Asset.YearOfValuation,
                AssignEmployeeId = _Asset.AssignEmployeeId,
                AssetStatus = _Asset.AssetStatus,
                IsAvilable = _Asset.IsAvilable,
                Note = _Asset.Note,
                Barcode = _Asset.Barcode,
                QRCode = _Asset.QRCode,
                QRCodeImage = _Asset.QRCodeImage,
                CreatedDate = _Asset.CreatedDate,
                ModifiedDate = _Asset.ModifiedDate,
                CreatedBy = _Asset.CreatedBy,
                ModifiedBy = _Asset.ModifiedBy,
                Cancelled = _Asset.Cancelled,
            };
        }

        public static implicit operator Asset(AssetCRUDViewModel vm)
        {
            return new Asset
            {
                Id = vm.Id,
                AssetId = vm.AssetId,
                AssetModelNo = vm.AssetModelNo,
                Name = vm.Name,
                Description = vm.Description,
                Category = vm.Category,
                SubCategory = vm.SubCategory,
                Quantity = vm.Quantity,
                UnitPrice = vm.UnitPrice,
                Supplier = vm.Supplier,
                Location = vm.Location,
                Department = vm.Department,
                SubDepartment = vm.SubDepartment,
                WarranetyInMonth = vm.WarranetyInMonth,
                DepreciationInMonth = vm.DepreciationInMonth,
                ImageURL = vm.ImageURL,
                PurchaseReceipt = vm.PurchaseReceipt,
                DateOfPurchase = vm.DateOfPurchase,
                DateOfManufacture = vm.DateOfManufacture,
                YearOfValuation = vm.YearOfValuation,
                AssignEmployeeId = vm.AssignEmployeeId,
                AssetStatus = vm.AssetStatus,
                IsAvilable = vm.IsAvilable,
                Note = vm.Note,
                Barcode = vm.Barcode,
                QRCode = vm.QRCode,
                QRCodeImage = vm.QRCodeImage,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
