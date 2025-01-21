using System;

namespace AMS.Models
{
    public class ManageUserRolesDetails : EntityBase
    {
        public Int64 Id { get; set; }
        public Int64 ManageRoleId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsAllowed { get; set; }      
    }
}
