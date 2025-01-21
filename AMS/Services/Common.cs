using Microsoft.EntityFrameworkCore;
using System.Net;
using UAParser;
using AMS.Data;
using AMS.Models;
using AMS.Models.CommonViewModel;
using AMS.Models.AssetViewModel;
using AMS.Models.UserProfileViewModel;
using AMS.Models.AssetHistoryViewModel;
using AMS.Models.CommentViewModel;
using AMS.Models.CompanyInfoViewModel;
using Microsoft.AspNetCore.Identity;
using AMS.Helpers;
using AMS.Models.AssetRequestViewModel;
using AMS.Models.AssetIssueViewModel;
using AMS.Models.ManageUserRolesVM;

namespace AMS.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        public Common(IWebHostEnvironment iHostingEnvironment, ApplicationDbContext context)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
        }
        public string UploadedFile(IFormFile _IFormFile)
        {
            string ProfilePictureFileName = null;

            if (_IFormFile != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload");

                if (_IFormFile.FileName == null)
                    ProfilePictureFileName = Guid.NewGuid().ToString() + "_" + "blank-person.png";
                else
                    ProfilePictureFileName = Guid.NewGuid().ToString() + "_" + _IFormFile.FileName;
                string filePath = Path.Combine(uploadsFolder, ProfilePictureFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    _IFormFile.CopyTo(fileStream);
                }
            }
            return ProfilePictureFileName;
        }

        public async Task<SMTPEmailSetting> GetSMTPEmailSetting()
        {
            return await _context.Set<SMTPEmailSetting>().Where(x => x.Id == 1).SingleOrDefaultAsync();
        }
        public async Task<SendGridSetting> GetSendGridEmailSetting()
        {
            return await _context.Set<SendGridSetting>().Where(x => x.Id == 1).SingleOrDefaultAsync();
        }

        public UserProfile GetByUserProfile(Int64 id)
        {
            return _context.UserProfile.Where(x => x.UserProfileId == id).SingleOrDefault();
        }
        public IQueryable<UserProfileCRUDViewModel> GetUserProfileDetails()
        {
            var result = (from vm in _context.UserProfile
                          join _Department in _context.Department on vm.Department equals _Department.Id
                          into _Department
                          from objDepartment in _Department.DefaultIfEmpty()
                          join _SubDepartment in _context.SubDepartment on vm.SubDepartment equals _SubDepartment.Id
                          into _SubDepartment
                          from objSubDepartment in _SubDepartment.DefaultIfEmpty()
                          join _Designation in _context.Designation on vm.Designation equals _Designation.Id
                          into _Designation
                          from objDesignation in _Designation.DefaultIfEmpty()
                          join _ManageRole in _context.ManageUserRoles on vm.RoleId equals _ManageRole.Id
                          into _ManageRole
                          from objManageRole in _ManageRole.DefaultIfEmpty()
                          where vm.Cancelled == false
                          select new UserProfileCRUDViewModel
                          {
                              UserProfileId = vm.UserProfileId,
                              ApplicationUserId = vm.ApplicationUserId,
                              EmployeeId = vm.EmployeeId,
                              FirstName = vm.FirstName,
                              LastName = vm.LastName,
                              DateOfBirth = vm.DateOfBirth,
                              Designation = vm.Designation,
                              DesignationDisplay = objDesignation.Name,
                              Department = vm.Department,
                              DepartmentDisplay = objDepartment.Name,
                              SubDepartment = vm.SubDepartment,
                              SubDepartmentDisplay = objSubDepartment.Name,
                              JoiningDate = vm.JoiningDate,
                              LeavingDate = vm.LeavingDate,
                              PhoneNumber = vm.PhoneNumber,
                              Email = vm.Email,
                              Address = vm.Address,
                              Country = vm.Country,
                              ProfilePicture = vm.ProfilePicture,
                              RoleId = vm.RoleId,
                              RoleIdDisplay = objManageRole.Name,
                              IsApprover = vm.IsApprover,
                              IsApproverDisplay = vm.IsApprover == 1 ? "No" : "Yes",
                              CreatedDate = vm.CreatedDate,
                              ModifiedDate = vm.ModifiedDate,
                              CreatedBy = vm.CreatedBy,
                              ModifiedBy = vm.ModifiedBy,
                              Cancelled = vm.Cancelled,
                          }).OrderByDescending(x => x.UserProfileId);
            return result;
        }
        public UserProfileCRUDViewModel GetByUserProfileInfo(Int64 id)
        {
            UserProfileCRUDViewModel _UserProfileCRUDViewModel = _context.UserProfile.Where(x => x.UserProfileId == id).SingleOrDefault();
            return _UserProfileCRUDViewModel;
        }
        public async Task<bool> InsertLoginHistory(LoginHistory _LoginHistory, ClientInfo _ClientInfo)
        {
            try
            {
                _LoginHistory.PublicIP = GetPublicIP();
                _LoginHistory.CreatedDate = DateTime.Now;
                _LoginHistory.ModifiedDate = DateTime.Now;

                _context.Add(_LoginHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetPublicIP()
        {
            try
            {
                string url = "http://checkip.dyndns.org/";
                WebRequest req = WebRequest.Create(url);
                WebResponse resp = req.GetResponse();
                StreamReader sr = new StreamReader(resp.GetResponseStream());
                string response = sr.ReadToEnd().Trim();
                string[] a = response.Split(':');
                string a2 = a[1].Substring(1);
                string[] a3 = a2.Split('<');
                string a4 = a3[0];
                return a4;
            }
            catch (Exception ex)
            {
                return ex.Message;
                throw new Exception("No network adapters with an IPv4 address in the system!");
            }
        }
        public IQueryable<ItemDropdownListViewModel> GetCommonddlData(string strTableName)
        {
            var sql = "select Id, Name from " + strTableName + " where Cancelled = 0";
            var result = _context.ItemDropdownListViewModel.FromSqlRaw(sql);
            return result;
        }
        public IEnumerable<T> GetTableData<T>(ApplicationDbContext dbContext) where T : class
        {
            return dbContext.Set<T>();
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlDepartment()
        {
            return (from tblObj in _context.Department.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlSubDepartment()
        {
            return (from tblObj in _context.SubDepartment.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlAssetCategorie()
        {
            return (from tblObj in _context.AssetCategorie.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlAssetSubCategorie()
        {
            return (from tblObj in _context.AssetSubCategorie.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlSupplier()
        {
            return (from tblObj in _context.Supplier.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlAssetStatus()
        {
            return (from tblObj in _context.AssetStatus.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                    select new ItemDropdownListViewModel
                    {
                        Id = tblObj.Id,
                        Name = tblObj.Name,
                    });
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlEmployee()
        {
            var result = (from tblObj in _context.UserProfile.Where(x => x.Cancelled == false).OrderBy(x => x.UserProfileId)
                          select new ItemDropdownListViewModel
                          {
                              Id = tblObj.UserProfileId,
                              Name = tblObj.FirstName + " " + tblObj.LastName,
                          });
            return result;
        }
        public IQueryable<ItemDropdownListViewModel> LoadddlDesignation()
        {
            var result = (from tblObj in _context.Designation.Where(x => x.Cancelled == false).OrderBy(x => x.Id)
                          select new ItemDropdownListViewModel
                          {
                              Id = tblObj.Id,
                              Name = tblObj.Name,
                          });
            return result;
        }
        public IQueryable<AssetCRUDViewModel> GetGridAssetList(bool _IsAdmin)
        {
            try
            {
                return (from _Asset in _context.Asset
                        join _UserProfile in _context.UserProfile on _Asset.AssignEmployeeId equals _UserProfile.UserProfileId
                        into listEmployee
                        from _Employee in listEmployee.DefaultIfEmpty()
                        where _Asset.Cancelled == false
                        select new AssetCRUDViewModel
                        {
                            Id = _Asset.Id,
                            AssetId = _Asset.AssetId,
                            AssignEmployeeId = _Asset.AssignEmployeeId,
                            AssetModelNo = _Asset.AssetModelNo,
                            Name = _Asset.Name,
                            AssignEmployeeDisplay = _Asset.AssignEmployeeId == 0 ? "Unassigned" : _Employee.FirstName + " " + _Employee.LastName,
                            UnitPrice = _Asset.UnitPrice,
                            DateOfPurchase = _Asset.DateOfPurchase,
                            ImageURL = _Asset.ImageURL,
                            IsAdmin = _IsAdmin
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<AssetCRUDViewModel> GetAssetList()
        {
            try
            {
                return (from _Asset in _context.Asset
                        join _AssetCategorie in _context.AssetCategorie on _Asset.Category equals _AssetCategorie.Id
                        into listAssetCategorie
                        from _AssetCategorie in listAssetCategorie.DefaultIfEmpty()
                        join _AssetSubCategorie in _context.AssetSubCategorie on _Asset.SubCategory equals _AssetSubCategorie.Id
                        into listAssetSubCategorie
                        from _AssetSubCategorie in listAssetSubCategorie.DefaultIfEmpty()
                        join _Department in _context.Department on _Asset.Department equals _Department.Id
                        into listDepartment
                        from _Department in listDepartment.DefaultIfEmpty()
                        join _SubDepartment in _context.SubDepartment on _Asset.SubDepartment equals _SubDepartment.Id
                        into listSubDepartment
                        from _SubDepartment in listSubDepartment.DefaultIfEmpty()
                        join _Supplier in _context.Supplier on _Asset.Supplier equals _Supplier.Id
                        into listSupplier
                        from _Supplier in listSupplier.DefaultIfEmpty()
                        join _AssetStatus in _context.AssetStatus on _Asset.AssetStatus equals _AssetStatus.Id
                        into listAssetStatus
                        from _AssetStatus in listAssetStatus.DefaultIfEmpty()
                            //where _Asset.Cancelled == false
                        select new AssetCRUDViewModel
                        {
                            Id = _Asset.Id,
                            AssetId = _Asset.AssetId,
                            AssetModelNo = _Asset.AssetModelNo,
                            Name = _Asset.Name,
                            Description = _Asset.Description,
                            Category = _Asset.Category,
                            CategoryDisplay = _AssetCategorie.Name,
                            SubCategory = _Asset.SubCategory,
                            SubCategoryDisplay = _AssetSubCategorie.Name,
                            Quantity = _Asset.Quantity,
                            UnitPrice = _Asset.UnitPrice,
                            Supplier = _Asset.Supplier,
                            SupplierDisplay = _Supplier.Name,
                            Location = _Asset.Location,
                            Department = _Asset.Department,
                            DepartmentDisplay = _Department.Name,
                            SubDepartment = _Asset.SubDepartment,
                            SubDepartmentDisplay = _SubDepartment.Name,
                            WarranetyInMonth = _Asset.WarranetyInMonth,
                            DepreciationInMonth = _Asset.DepreciationInMonth,
                            ImageURL = _Asset.ImageURL,
                            PurchaseReceipt = Path.GetFileName(_Asset.PurchaseReceipt),
                            DateOfPurchase = _Asset.DateOfPurchase,
                            DateOfManufacture = _Asset.DateOfManufacture,
                            YearOfValuation = _Asset.YearOfValuation,
                            AssignEmployeeId = _Asset.AssignEmployeeId,
                            AssetStatus = _Asset.AssetStatus,
                            AssetStatusDisplay = _AssetStatus.Name,
                            IsAvilable = _Asset.IsAvilable,
                            Note = _Asset.Note,
                            Barcode = _Asset.Barcode,
                            QRCode = _Asset.QRCode,
                            QRCodeImage = _Asset.QRCodeImage,
                            CreatedDate = _Asset.CreatedDate,
                            ModifiedDate = _Asset.ModifiedDate,
                            CreatedBy = _Asset.CreatedBy,
                            ModifiedBy = _Asset.ModifiedBy,
                            Cancelled = _Asset.Cancelled,
                        }).OrderByDescending(x => x.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<AssetHistory> AddAssetHistory(AssetHistoryCRUDViewModel vm)
        {
            try
            {
                AssetHistory _AssetHistory = new AssetHistory();
                _AssetHistory = vm;
                _AssetHistory.CreatedDate = DateTime.Now;
                _AssetHistory.ModifiedDate = DateTime.Now;
                _AssetHistory.CreatedBy = vm.UserName;
                _AssetHistory.ModifiedBy = vm.UserName;
                _context.Add(_AssetHistory);
                await _context.SaveChangesAsync();

                return _AssetHistory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<AssetHistoryCRUDViewModel> GetAssetHistoryList()
        {
            try
            {
                var result = (from _AssetHistory in _context.AssetHistory
                              join _Asset in _context.Asset on _AssetHistory.AssetId equals _Asset.Id
                              into listAsset
                              from _Asset in listAsset.DefaultIfEmpty()
                              join _UserProfile in _context.UserProfile on _AssetHistory.AssignEmployeeId equals _UserProfile.UserProfileId
                              into listEmployee
                              from _Employee in listEmployee.DefaultIfEmpty()
                              where _AssetHistory.Cancelled == false
                              select new AssetHistoryCRUDViewModel
                              {
                                  Id = _AssetHistory.Id,
                                  AssetId = _AssetHistory.AssetId,
                                  AssetDisplay = _Asset.Name,
                                  Action = _AssetHistory.Action,
                                  AssignEmployeeId = _AssetHistory.AssignEmployeeId,
                                  AssignEmployeeDisplay = _AssetHistory.AssignEmployeeId == 0 ? "Unassigned" : _Employee.FirstName + " " + _Employee.LastName,
                                  Note = _AssetHistory.Note,
                                  CreatedDate = _AssetHistory.CreatedDate,
                                  CreatedDateDisplay = String.Format("{0:f}", _AssetHistory.CreatedDate),
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<CommentCRUDViewModel> GetCommentList()
        {
            try
            {
                return (from _Comment in _context.Comment
                        join _Asset in _context.Asset on _Comment.AssetId equals _Asset.Id
                        select new CommentCRUDViewModel
                        {
                            Id = _Comment.Id,
                            AssetId = _Comment.AssetId,
                            AssetName = _Asset.Name,
                            Message = _Comment.Message,
                            IsDeleted = _Comment.IsDeleted,
                            IsDeletedDisplay = _Comment.IsDeleted == true ? "Yes" : "No",
                            CreatedDate = _Comment.CreatedDate,
                            ModifiedDate = _Comment.ModifiedDate,
                            CreatedBy = _Comment.CreatedBy,
                        }).OrderBy(x => x.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<CommentCRUDViewModel> GetCommentList(Int64 AssetId)
        {
            try
            {
                return (from _Comment in _context.Comment
                        join _Asset in _context.Asset on _Comment.AssetId equals _Asset.Id
                        where _Comment.Cancelled == false && _Asset.Id == AssetId
                        select new CommentCRUDViewModel
                        {
                            Id = _Comment.Id,
                            AssetId = _Comment.AssetId,
                            AssetName = _Asset.Name,
                            Message = _Comment.Message,
                            IsDeleted = _Comment.IsDeleted,
                            CreatedDate = _Comment.CreatedDate,
                            ModifiedDate = _Comment.ModifiedDate,
                            CreatedBy = _Comment.CreatedBy,
                        }).OrderBy(x => x.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<ScannerCodeViewModel> GetBarcodeList()
        {
            var result = (from _Asset in _context.Asset
                          where _Asset.Cancelled == false
                          select new ScannerCodeViewModel
                          {
                              Id = _Asset.Id,
                              AssetName = _Asset.Name,
                              Barcode = _Asset.Barcode,
                          }).OrderBy(x => x.Id);
            return result;
        }
        public IQueryable<ScannerCodeViewModel> GetQRcodeList()
        {
            var result = (from _Asset in _context.Asset
                          where _Asset.Cancelled == false
                          select new ScannerCodeViewModel
                          {
                              Id = _Asset.Id,
                              AssetName = _Asset.Name,
                              Barcode = _Asset.QRCodeImage,
                          }).OrderBy(x => x.Id);
            return result;
        }

        public CompanyInfoCRUDViewModel GetCompanyInfo()
        {
            CompanyInfoCRUDViewModel vm = _context.CompanyInfo.FirstOrDefault(m => m.Id == 1);
            return vm;
        }
        public async Task<Int64> GetLoginEmployeeId(string _UserEmail)
        {
            Int64 _UserProfileId = 0;
            var _ApplicationUser = await _context.ApplicationUser.Where(x => x.Email == _UserEmail).SingleOrDefaultAsync();
            if (_ApplicationUser != null)
            {
                _UserProfileId = _context.UserProfile.Where(x => x.ApplicationUserId == _ApplicationUser.Id).SingleOrDefault().UserProfileId;
            }
            return _UserProfileId;
        }
        public IQueryable<AssetAssignedCRUDViewModel> GetAssetAssignedList(Int64 _EmployeeId)
        {
            try
            {
                var result = (from _AssetAssigned in _context.AssetAssigned
                              join _Asset in _context.Asset on _AssetAssigned.AssetId equals _Asset.Id
                              join _UserProfile in _context.UserProfile on _AssetAssigned.EmployeeId equals _UserProfile.UserProfileId
                              where _AssetAssigned.Cancelled == false && _AssetAssigned.EmployeeId == _EmployeeId && _AssetAssigned.Status == AssetAssignedStatus.Assigned
                              select new AssetAssignedCRUDViewModel
                              {
                                  Id = _AssetAssigned.Id,
                                  AssetId = _AssetAssigned.AssetId,
                                  AssetName = _Asset.Name,
                                  EmployeeId = _AssetAssigned.EmployeeId,
                                  EmployeeName = _UserProfile.FirstName + " " + _UserProfile.LastName,
                                  Status = _AssetAssigned.Status,

                                  CreatedDate = _AssetAssigned.CreatedDate,
                                  ModifiedDate = _AssetAssigned.ModifiedDate,
                                  CreatedBy = _AssetAssigned.CreatedBy,
                                  ModifiedBy = _AssetAssigned.ModifiedBy,
                              }).OrderBy(x => x.Id);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<AssetAssignedCRUDViewModel> GetAssetAssignedListAllStatus(Int64 _EmployeeId)
        {
            try
            {
                var result = (from _AssetAssigned in _context.AssetAssigned
                              join _Asset in _context.Asset on _AssetAssigned.AssetId equals _Asset.Id
                              join _UserProfile in _context.UserProfile on _AssetAssigned.EmployeeId equals _UserProfile.UserProfileId
                              where _AssetAssigned.Cancelled == false && _AssetAssigned.EmployeeId == _EmployeeId && _Asset.Cancelled == false
                              select new AssetAssignedCRUDViewModel
                              {
                                  Id = _AssetAssigned.Id,
                                  AssetId = _AssetAssigned.AssetId,
                                  AssetName = _Asset.Name,
                                  EmployeeId = _AssetAssigned.EmployeeId,
                                  EmployeeName = _UserProfile.FirstName + " " + _UserProfile.LastName,
                                  Status = _AssetAssigned.Status,

                                  CreatedDate = _AssetAssigned.CreatedDate,
                                  ModifiedDate = _AssetAssigned.ModifiedDate,
                                  CreatedBy = _AssetAssigned.CreatedBy,
                                  ModifiedBy = _AssetAssigned.ModifiedBy,
                              }).OrderBy(x => x.Id);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DownloadPurchaseReceiptViewModel GetDownloadDetails(Int64 id)
        {
            DownloadPurchaseReceiptViewModel vm = new();
            try
            {
                var _Asset = _context.Asset.Where(x => x.Id == id).SingleOrDefault();
                string _WebRootPath = _iHostingEnvironment.WebRootPath + _Asset.PurchaseReceipt;
                using (var _MemoryStream = new MemoryStream())
                {
                    using (FileStream file = new FileStream(_WebRootPath, FileMode.Open, FileAccess.Read))
                        file.CopyTo(_MemoryStream);
                    vm.DocByte = _MemoryStream.ToArray();
                }
                //string _GetExtension = Path.GetExtension(_Asset.PurchaseReceipt);
                vm.FileName = Path.GetFileName(_Asset.PurchaseReceipt);
                vm.ContentType = "application/octet-stream";
                return vm;
            }
            catch (Exception) { throw; }
        }
        public IQueryable<AssetRequestCRUDViewModel> GetAssetRequestList(bool _IsAdmin)
        {
            try
            {
                var result = (from _AssetRequest in _context.AssetRequest
                              join _Asset in _context.Asset on _AssetRequest.AssetId equals _Asset.Id
                              join _RequestedEmployee in _context.UserProfile on _AssetRequest.RequestedEmployeeId equals _RequestedEmployee.UserProfileId
                              join _ApprovedByEmployee in _context.UserProfile on _AssetRequest.ApprovedByEmployeeId equals _ApprovedByEmployee.UserProfileId
                              where _AssetRequest.Cancelled == false
                              select new AssetRequestCRUDViewModel
                              {
                                  Id = _AssetRequest.Id,
                                  AssetId = _AssetRequest.AssetId,
                                  AssetDisplay = _Asset.Name,
                                  RequestedEmployeeId = _AssetRequest.RequestedEmployeeId,
                                  RequestedEmployeeDisplay = _RequestedEmployee.FirstName + " " + _RequestedEmployee.LastName,
                                  ApprovedByEmployeeId = _AssetRequest.ApprovedByEmployeeId,
                                  ApprovedByEmployeeDisplay = _ApprovedByEmployee.FirstName + " " + _ApprovedByEmployee.LastName,
                                  RequestDetails = _AssetRequest.RequestDetails,
                                  Status = _AssetRequest.Status,
                                  RequestDate = _AssetRequest.RequestDate,
                                  ReceiveDate = _AssetRequest.ReceiveDate,
                                  Comment = _AssetRequest.Comment,

                                  CreatedDate = _AssetRequest.CreatedDate,
                                  ModifiedDate = _AssetRequest.ModifiedDate,
                                  CreatedBy = _AssetRequest.CreatedBy,
                                  ModifiedBy = _AssetRequest.ModifiedBy,
                                  Cancelled = _AssetRequest.Cancelled,
                                  IsAdmin = _IsAdmin                                
                              }).OrderByDescending(x => x.Id);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IQueryable<AssetIssueCRUDViewModel> GetAssetIssueList(bool _IsAdmin)
        {
            try
            {
                var result = (from _AssetIssue in _context.AssetIssue
                              join _Asset in _context.Asset on _AssetIssue.AssetId equals _Asset.Id
                              join _RaisedByEmployee in _context.UserProfile on _AssetIssue.RaisedByEmployeeId equals _RaisedByEmployee.UserProfileId
                              where _AssetIssue.Cancelled == false
                              select new AssetIssueCRUDViewModel
                              {
                                  Id = _AssetIssue.Id,
                                  AssetId = _AssetIssue.AssetId,
                                  AssetDisplay = _Asset.Name,
                                  RaisedByEmployeeId = _AssetIssue.RaisedByEmployeeId,
                                  RaisedByEmployeeDisplay = _RaisedByEmployee.FirstName + " " + _RaisedByEmployee.LastName,
                                  IssueDescription = _AssetIssue.IssueDescription,
                                  Status = _AssetIssue.Status,
                                  ExpectedFixDate = _AssetIssue.ExpectedFixDate,
                                  ResolvedDate = _AssetIssue.ResolvedDate,
                                  Comment = _AssetIssue.Comment,
                                  CreatedDate = _AssetIssue.CreatedDate,
                                  ModifiedDate = _AssetIssue.ModifiedDate,
                                  CreatedBy = _AssetIssue.CreatedBy,
                                  ModifiedBy = _AssetIssue.ModifiedBy,
                                  Cancelled = _AssetIssue.Cancelled,
                                  IsAdmin = _IsAdmin
                              }).OrderByDescending(x => x.Id);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ManageUserRolesDetails>> GetManageRoleDetailsList(Int64 id)
        {
            var result = await (from tblObj in _context.ManageUserRolesDetails.Where(x => x.ManageRoleId == id)
                                select new ManageUserRolesDetails
                                {
                                    Id = tblObj.Id,
                                    RoleId = tblObj.RoleId,
                                    RoleName = tblObj.RoleName,
                                    IsAllowed = tblObj.IsAllowed,
                                }).OrderBy(x => x.RoleName).ToListAsync();
            return result;
        }
    }
}