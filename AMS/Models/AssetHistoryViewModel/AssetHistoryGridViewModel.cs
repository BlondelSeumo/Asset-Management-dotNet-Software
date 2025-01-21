using System;

namespace AMS.Models.AssetHistoryViewModel
{
    public class AssetHistoryGridViewModel : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 AssetId { get; set; }
        public string Action { get; set; }
        public string AssignUserId { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Note { get; set; }
    }
}
