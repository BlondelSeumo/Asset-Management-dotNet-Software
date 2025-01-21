using System;

namespace AMS.Models.AssetIssueViewModel
{
    public class AssetIssueGridViewModel : EntityBase
    {
                public Int64 Id { get; set; }
        public Int64 AssetId { get; set; }
        public Int64 RaisedByEmployeeId { get; set; }
        public string IssueDescription { get; set; }
        public string Status { get; set; }
        public DateTime ExpectedFixDate { get; set; }
        public DateTime ResolvedDate { get; set; }
        public string Comment { get; set; }


    }
}

