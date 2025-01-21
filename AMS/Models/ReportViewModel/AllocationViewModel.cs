using System;

namespace AMS.Models.ReportViewModel
{
    public class AllocationViewModel
    {
        public Int64 EmployeeId { get; set; }
        public string UserName { get; set; }
        public int TotalAssigned { get; set; }
        public int TotalUnassigned { get; set; }
        public int TotalAsset { get; set; }
    }
}
