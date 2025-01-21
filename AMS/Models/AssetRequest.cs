using System;

namespace AMS.Models
{
    public class AssetRequest : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 AssetId { get; set; }
        public Int64 RequestedEmployeeId { get; set; }
        public Int64 ApprovedByEmployeeId { get; set; }
        public string RequestDetails { get; set; }
        public string Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string Comment { get; set; }
    }
}
