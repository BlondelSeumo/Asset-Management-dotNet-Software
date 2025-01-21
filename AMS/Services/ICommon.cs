using UAParser;
using AMS.Data;
using AMS.Models;
using AMS.Models.UserProfileViewModel;
using AMS.Models.CommonViewModel;
using AMS.Models.AssetViewModel;
using AMS.Models.AssetHistoryViewModel;
using AMS.Models.CommentViewModel;
using AMS.Models.CompanyInfoViewModel;
using AMS.Models.AssetRequestViewModel;
using AMS.Models.AssetIssueViewModel;
using AMS.Models.ManageUserRolesVM;

namespace AMS.Services
{
    public interface ICommon
    {
        string UploadedFile(IFormFile ProfilePicture);
        Task<SMTPEmailSetting> GetSMTPEmailSetting();
        Task<SendGridSetting> GetSendGridEmailSetting();
        UserProfile GetByUserProfile(Int64 id);
        IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails();
        UserProfileCRUDViewModel GetByUserProfileInfo(Int64 id);
        Task<bool> InsertLoginHistory(LoginHistory _LoginHistory, ClientInfo _ClientInfo);
        IQueryable<ItemDropdownListViewModel> GetCommonddlData(string strTableName);
        IEnumerable<T> GetTableData<T>(ApplicationDbContext dbContext) where T : class;
        IQueryable<ItemDropdownListViewModel> LoadddlDepartment();
        IQueryable<ItemDropdownListViewModel> LoadddlSubDepartment();
        IQueryable<ItemDropdownListViewModel> LoadddlAssetCategorie();
        IQueryable<ItemDropdownListViewModel> LoadddlAssetSubCategorie();
        IQueryable<ItemDropdownListViewModel> LoadddlSupplier();
        IQueryable<ItemDropdownListViewModel> LoadddlAssetStatus();
        IQueryable<ItemDropdownListViewModel> LoadddlEmployee();
        IQueryable<ItemDropdownListViewModel> LoadddlDesignation();
        IQueryable<AssetCRUDViewModel> GetAssetList();
        IQueryable<AssetCRUDViewModel> GetGridAssetList(bool _IsAdmin);
        Task<AssetHistory> AddAssetHistory(AssetHistoryCRUDViewModel vm);
        IQueryable<AssetHistoryCRUDViewModel> GetAssetHistoryList();
        IQueryable<CommentCRUDViewModel> GetCommentList();
        IQueryable<CommentCRUDViewModel> GetCommentList(Int64 AssetId);
        IQueryable<ScannerCodeViewModel> GetBarcodeList();
        IQueryable<ScannerCodeViewModel> GetQRcodeList();
        CompanyInfoCRUDViewModel GetCompanyInfo();
        Task<Int64> GetLoginEmployeeId(string _UserEmail);
        IQueryable<AssetAssignedCRUDViewModel> GetAssetAssignedList(Int64 _EmployeeId);
        IQueryable<AssetAssignedCRUDViewModel> GetAssetAssignedListAllStatus(Int64 _EmployeeId);
        DownloadPurchaseReceiptViewModel GetDownloadDetails(Int64 id);
        IQueryable<AssetRequestCRUDViewModel> GetAssetRequestList(bool _IsAdmin);
        IQueryable<AssetIssueCRUDViewModel> GetAssetIssueList(bool _IsAdmin);
        Task<List<ManageUserRolesDetails>> GetManageRoleDetailsList(Int64 id);
    }
}
