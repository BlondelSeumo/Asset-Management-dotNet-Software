namespace AMS.Pages
{
    public class MainMenuViewModel
    {
        public bool Admin { get; set; }
        public bool SuperAdmin { get; set; }
        public bool Dashboard { get; set; }
        public bool UserManagement { get; set; }
        public bool UserInfoFromBrowser { get; set; }
        public bool AuditLogs { get; set; }
        public bool SubscriptionRequest { get; set; }
        public bool UserProfile { get; set; }
        public bool EmailSetting { get; set; }
        public bool IdentitySetting { get; set; }
        public bool LoginHistory { get; set; }

        //AMS
        public bool Asset { get; set; }
        public bool AssetHistory { get; set; }
        public bool Comment { get; set; }
        public bool PrintBarcode { get; set; }
        public bool PrintQRcode { get; set; }
        public bool Employee { get; set; }
        public bool Designation { get; set; }
        public bool AssetCategorie { get; set; }
        public bool AssetSubCategorie { get; set; }
        public bool AssetStatus { get; set; }
        public bool Supplier { get; set; }
        public bool Department { get; set; }
        public bool SubDepartment { get; set; }
        public bool CompanyInfo { get; set; }
        public bool AssetStatusReport { get; set; }
        public bool AssetAllocationReport { get; set; }
        public bool RequestModule { get; set; }
        public bool AssetRequest { get; set; }
        public bool AssetIssue { get; set; }
        public bool Categories { get; set; }
        public bool SystemRole { get; set; }
        public bool ManageUserRoles { get; set; }
    }
}