using System;

namespace AMS.Models
{
    public class AssetAssigned : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 AssetId { get; set; }
        public Int64 EmployeeId { get; set; }
        public string Status { get; set; }
    }
}
