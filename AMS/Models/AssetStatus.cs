using System;

namespace AMS.Models
{
    public class AssetStatus : EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }      
    }
}
