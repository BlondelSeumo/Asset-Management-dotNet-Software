using System;

namespace AMS.Models
{
    public class SubDepartment : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }      
    }
}
