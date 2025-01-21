using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.Models.AssetHistoryViewModel
{
    public class AssetHistoryCRUDViewModel : EntityBase
    {
        [Display(Name = "SL")]
        [Required]
        public Int64 Id { get; set; }
        public Int64 AssetId { get; set; }
        public string AssetDisplay { get; set; }
        public Int64 AssignEmployeeId { get; set; }
        public string AssignEmployeeDisplay { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public string CreatedDateDisplay { get; set; }


        public static implicit operator AssetHistoryCRUDViewModel(AssetHistory _AssetHistory)
        {
            return new AssetHistoryCRUDViewModel
            {
                Id = _AssetHistory.Id,
                AssetId = _AssetHistory.AssetId,
                AssignEmployeeId = _AssetHistory.AssignEmployeeId,
                Action = _AssetHistory.Action,
                Note = _AssetHistory.Note,
                CreatedDate = _AssetHistory.CreatedDate,
                ModifiedDate = _AssetHistory.ModifiedDate,
                CreatedBy = _AssetHistory.CreatedBy,
                ModifiedBy = _AssetHistory.ModifiedBy,
                Cancelled = _AssetHistory.Cancelled,
            };
        }

        public static implicit operator AssetHistory(AssetHistoryCRUDViewModel vm)
        {
            return new AssetHistory
            {
                Id = vm.Id,
                AssetId = vm.AssetId,
                AssignEmployeeId = vm.AssignEmployeeId,
                Action = vm.Action,
                Note = vm.Note,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
