namespace AMS.Models.ManageUserRolesVM
{
    public class ManageUserRolesViewModel
    {
        public Int64 ManageRoleDetailsId { get; set; }
        public string RoleId { get; set; }
        public Int64 RoleId_SL { get; set; }
        public string RoleName { get; set; }
        public bool IsAllowed { get; set; }
    }

    public class UpdateRoleViewModel
    {
        public string ApplicationUserId { get; set; }
        public string CurrentURL { get; set; }
        public List<ManageUserRolesViewModel> listManageUserRolesViewModel { get; set; }
    }
}
