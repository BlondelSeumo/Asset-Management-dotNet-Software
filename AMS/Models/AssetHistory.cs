using System;

namespace AMS.Models
{
    public class AssetHistory : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 AssetId { get; set; }
        public Int64 AssignEmployeeId { get; set; }
        public string Action { get; set; }
        public string Note { get; set; }
    }
}
