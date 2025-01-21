using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models.AssetRequestViewModel
{
    public class AssetRequestCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Asset Name")]
        [Required]
        public Int64 AssetId { get; set; }
        public string AssetDisplay { get; set; }
        [Display(Name = "Requested Employee")]
        [Required]
        public Int64 RequestedEmployeeId { get; set; }
        public string RequestedEmployeeDisplay { get; set; }
        [Display(Name = "Approved By Employee")]
        [Required]
        public Int64 ApprovedByEmployeeId { get; set; }
        public string ApprovedByEmployeeDisplay { get; set; }
        [Display(Name = "Request Details")]
        [Required]
        public string RequestDetails { get; set; }
        public string Status { get; set; } = "New";
        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.Now;
        [Display(Name = "Receive Date")]
        public DateTime ReceiveDate { get; set; } = DateTime.Now.AddDays(7);
        public string Comment { get; set; }
        public bool IsAdmin { get; set; }

        public static implicit operator AssetRequestCRUDViewModel(AssetRequest _AssetRequest)
        {
            return new AssetRequestCRUDViewModel
            {
                Id = _AssetRequest.Id,
                AssetId = _AssetRequest.AssetId,
                RequestedEmployeeId = _AssetRequest.RequestedEmployeeId,
                ApprovedByEmployeeId = _AssetRequest.ApprovedByEmployeeId,
                RequestDetails = _AssetRequest.RequestDetails,
                Status = _AssetRequest.Status,
                RequestDate = _AssetRequest.RequestDate,
                ReceiveDate = _AssetRequest.ReceiveDate,
                Comment = _AssetRequest.Comment,
                CreatedDate = _AssetRequest.CreatedDate,
                ModifiedDate = _AssetRequest.ModifiedDate,
                CreatedBy = _AssetRequest.CreatedBy,
                ModifiedBy = _AssetRequest.ModifiedBy,
                Cancelled = _AssetRequest.Cancelled,
            };
        }

        public static implicit operator AssetRequest(AssetRequestCRUDViewModel vm)
        {
            return new AssetRequest
            {
                Id = vm.Id,
                AssetId = vm.AssetId,
                RequestedEmployeeId = vm.RequestedEmployeeId,
                ApprovedByEmployeeId = vm.ApprovedByEmployeeId,
                RequestDetails = vm.RequestDetails,
                Status = vm.Status,
                RequestDate = vm.RequestDate,
                ReceiveDate = vm.ReceiveDate,
                Comment = vm.Comment,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
