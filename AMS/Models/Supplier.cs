using System;

namespace AMS.Models
{
    public class Supplier : EntityBase
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
