using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models.AssetIssueViewModel
{
    public class AssetIssueCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        [Display(Name = "Asset")]
        [Required]
        public Int64 AssetId { get; set; }
        public string AssetDisplay { get; set; }
        [Display(Name = "Raised By Employee")]
        [Required]
        public Int64 RaisedByEmployeeId { get; set; }
        public string RaisedByEmployeeDisplay { get; set; }
        [Display(Name = "Issue Description")]
        [Required]
        public string IssueDescription { get; set; }
        public string Status { get; set; } = "New";
        [Display(Name = "Expected Fix Date")]
        public DateTime ExpectedFixDate { get; set; } = DateTime.Now.AddDays(7);
        [Display(Name = "Resolved Date")]
        public DateTime ResolvedDate { get; set; } = DateTime.Now.AddDays(7);
        public string Comment { get; set; }
        public bool IsAdmin { get; set; }

        public static implicit operator AssetIssueCRUDViewModel(AssetIssue _AssetIssue)
        {
            return new AssetIssueCRUDViewModel
            {
                Id = _AssetIssue.Id,
                AssetId = _AssetIssue.AssetId,
                RaisedByEmployeeId = _AssetIssue.RaisedByEmployeeId,
                IssueDescription = _AssetIssue.IssueDescription,
                Status = _AssetIssue.Status,
                ExpectedFixDate = _AssetIssue.ExpectedFixDate,
                ResolvedDate = _AssetIssue.ResolvedDate,
                Comment = _AssetIssue.Comment,
                
                CreatedDate = _AssetIssue.CreatedDate,
                ModifiedDate = _AssetIssue.ModifiedDate,
                CreatedBy = _AssetIssue.CreatedBy,
                ModifiedBy = _AssetIssue.ModifiedBy,
                Cancelled = _AssetIssue.Cancelled,
            };
        }

        public static implicit operator AssetIssue(AssetIssueCRUDViewModel vm)
        {
            return new AssetIssue
            {
                Id = vm.Id,
                AssetId = vm.AssetId,
                RaisedByEmployeeId = vm.RaisedByEmployeeId,
                IssueDescription = vm.IssueDescription,
                Status = vm.Status,
                ExpectedFixDate = vm.ExpectedFixDate,
                ResolvedDate = vm.ResolvedDate,
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
