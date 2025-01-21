using System;
using AMS.Helpers;

namespace AMS.Models
{
    public class AssetAssignedCRUDViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 AssetId { get; set; }
        public string AssetName { get; set; }
        public Int64 EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Status { get; set; } = AssetAssignedStatus.Assigned;
        public int StatusDisplay { get; set; }
        public List<AssetAssignedCRUDViewModel> listAssetAssignedCRUDViewModel { get; set; } 

        public static implicit operator AssetAssignedCRUDViewModel(AssetAssigned vm)
        {
            return new AssetAssignedCRUDViewModel
            {
                Id = vm.Id,
                AssetId = vm.Id,
                EmployeeId = vm.EmployeeId,
                Status = vm.Status,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
        public static implicit operator AssetAssigned(AssetAssignedCRUDViewModel vm)
        {
            return new AssetAssigned
            {
                Id = vm.Id,
                AssetId = vm.Id,
                EmployeeId = vm.EmployeeId,
                Status = vm.Status,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
        
        public static implicit operator AssetAssignedCRUDViewModel(Asset vm)
        {
            return new AssetAssignedCRUDViewModel
            {
                Id = vm.Id,
                AssetId = vm.Id,
                EmployeeId = vm.AssignEmployeeId,

                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
