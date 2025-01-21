using AMS.Helpers;
using AMS.Models;
using AMS.Models.UserProfileViewModel;

namespace AMS.Data
{
    public class SeedData
    {
        public IEnumerable<Asset> GetAssetList()
        {
            return new List<Asset>
            {
                new Asset { AssetModelNo = "HPLaptop101", Name = "HP Laptop 101", UnitPrice = 2500, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-7), ImageURL = "/images/DefaultAsset/HP_Laptop.jpg", AssetStatus = AssetStatusValue.New, Category = 1 },
                new Asset { AssetModelNo = "HPLaptop102", Name = "HP Laptop 102", UnitPrice = 2500, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-7), ImageURL = "/images/DefaultAsset/HP_Laptop.jpg", AssetStatus = AssetStatusValue.New, Category = 1 },
                new Asset { AssetModelNo = "HPLaptop103", Name = "HP Laptop 103", UnitPrice = 2500, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-7), ImageURL = "/images/DefaultAsset/HP_Laptop.jpg", AssetStatus = AssetStatusValue.New, Category = 1 },
                new Asset { AssetModelNo = "HPLaptop104", Name = "HP Laptop 104", UnitPrice = 2500, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-7), ImageURL = "/images/DefaultAsset/HP_Laptop.jpg", AssetStatus = AssetStatusValue.Available, Category = 1 },
                new Asset { AssetModelNo = "HPLaptop105", Name = "HP Laptop 105", UnitPrice = 2500, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-7), ImageURL = "/images/DefaultAsset/HP_Laptop.jpg", AssetStatus = AssetStatusValue.Available, Category = 1 },
                
                new Asset { AssetModelNo = "M1 Chip", Name = "Macbook Pro m1", UnitPrice = 2500, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-6), ImageURL = "/images/DefaultAsset/Macbook_Pro_m1.jpg", AssetStatus = AssetStatusValue.New, Category = 1 },
                new Asset { AssetModelNo = "HP123", Name = "HP Laptop", UnitPrice = 900, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-12), ImageURL = "/images/DefaultAsset/HP_Pavilion_13.jpg", AssetStatus = AssetStatusValue.New, Category = 1 },
                new Asset { AssetModelNo = "Samsung123", Name = "Samsung Curved Monitor", UnitPrice = 1200, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-5), ImageURL = "/images/DefaultAsset/Samsung_Curved_Monitor.jpg", AssetStatus = AssetStatusValue.Available, Category = 1 },
                new Asset { AssetModelNo = "WD123", Name = "WD Portable HD", UnitPrice = 800, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-2), ImageURL = "/images/DefaultAsset/WD_Portable_HD.jpg", AssetStatus = AssetStatusValue.Available, Category = 1 },
                new Asset { AssetModelNo = "iPhone123", Name = "iPhone X", UnitPrice = 1800, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-15), ImageURL = "/images/DefaultAsset/iPhone_X.jpg", AssetStatus = AssetStatusValue.Expired, Category = 1 },
                new Asset { AssetModelNo = "SamsungNote123", Name = "Samsung Note-20", UnitPrice = 2000, DateOfPurchase = DateTime.Now, DateOfManufacture = DateTime.Now.AddMonths(-7), ImageURL = "/images/DefaultAsset/Samsung_Note_20.jpg", AssetStatus = AssetStatusValue.Damage, Category = 1 },
            };
        }
        public IEnumerable<Supplier> GetSupplierList()
        {
            return new List<Supplier>
            {
                new Supplier { Name = "Common Supplier", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="Dhaka"},
                new Supplier { Name = "Google", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="USA"},
                new Supplier { Name = "Amazon", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="USA"},
                new Supplier { Name = "Microsoft", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="USA"},
                new Supplier { Name = "PHP", ContactPerson = "TBD", Email="dev@gmail.com" ,Phone="01699000",Address="Dhaka"}
            };
        }
        public IEnumerable<AssetCategorie> GetAssetCategorieList()
        {
            return new List<AssetCategorie>
            {
                new AssetCategorie { Name = "IT", Description = "IT Accessories"},
                new AssetCategorie { Name = "Electronics", Description = "All Electronics"},
                new AssetCategorie { Name = "Furniture", Description = "Office Furniture"},
                new AssetCategorie { Name = "Miscellaneous", Description = "Miscellaneous"},
                new AssetCategorie { Name = "Software", Description = "All Kind's Software Paid Application"},
            };
        }
        public IEnumerable<AssetSubCategorie> GetAssetSubCategorieList()
        {
            return new List<AssetSubCategorie>
            {
                new AssetSubCategorie { AssetCategorieId = 1, Name = "Destop Computer", Description = "Destop Computer"},
                new AssetSubCategorie { AssetCategorieId = 1,  Name = "Laptop", Description = "All Laptop Computer"},
                new AssetSubCategorie { AssetCategorieId = 3,  Name = "Office Chair", Description = "Office Chair"},
                new AssetSubCategorie { AssetCategorieId = 1,  Name = "Pendrive", Description = "Pendrive"},
                new AssetSubCategorie { AssetCategorieId = 1,  Name = "Charger", Description = "All Kind's Charger"},
            };
        }
        public IEnumerable<AssetStatus> GetAssetStatusList()
        {
            return new List<AssetStatus>
            {
                new AssetStatus { Name = "New", Description = "TBD"},
                new AssetStatus { Name = "In Use", Description = "TBD"},
                new AssetStatus { Name = "Available", Description = "TBD"},
                new AssetStatus { Name = "Damage", Description = "Damage"},
                new AssetStatus { Name = "Return", Description = "Return"},
                new AssetStatus { Name = "Expired", Description = "Expired"},
                new AssetStatus { Name = "Required License Update", Description = "TBD"},
                new AssetStatus { Name = "Miscellaneous", Description = "Miscellaneous"},
            };
        }

        public IEnumerable<Department> GetDepartmentList()
        {
            return new List<Department>
            {
                new Department { Name = "IT", Description = "IT Department"},
                new Department { Name = "HR", Description = "HR Department"},
                new Department { Name = "Finance", Description = "Finance Department"},
                new Department { Name = "Procurement", Description = "Procurement Department"},
                new Department { Name = "Legal", Description = "Procurement Department"},
            };
        }
        public IEnumerable<SubDepartment> GetSubDepartmentList()
        {
            return new List<SubDepartment>
            {
                new SubDepartment { DepartmentId = 1, Name = "QA", Description = "QA Department"},
                new SubDepartment { DepartmentId = 1, Name = "Software Development", Description = "Software Development Department"},
                new SubDepartment { DepartmentId = 1, Name = "Operation", Description = "Operation Department"},
                new SubDepartment { DepartmentId = 1, Name = "PM", Description = "Project Management Department"},
                new SubDepartment { DepartmentId = 2, Name = "Recruitment", Description = "Recruitment Department"},
            };
        }
        public IEnumerable<Designation> GetDesignationList()
        {
            return new List<Designation>
            {
                new Designation { Name = "Project Manager", Description = "Employee Job Designation"},
                new Designation { Name = "Software Engineer", Description = "Employee Job Designation"},
                new Designation { Name = "Head Of Engineering", Description = "Employee Job Designation"},
                new Designation { Name = "Software Architect", Description = "Employee Job Designation"},
                new Designation { Name = "QA Engineer", Description = "Employee Job Designation"},
                new Designation { Name = "DevOps Engineer", Description = "Employee Job Designation"},
            };
        }
        public IEnumerable<AssetRequest> GetAssetRequestList()
        {
            return new List<AssetRequest>
            {
                new AssetRequest { AssetId = 1, RequestedEmployeeId = 1, ApprovedByEmployeeId = 2, Status = "New"},
                new AssetRequest { AssetId = 2, RequestedEmployeeId = 2, ApprovedByEmployeeId = 5, Status = "New"},
                new AssetRequest { AssetId = 3, RequestedEmployeeId = 3, ApprovedByEmployeeId = 2, Status = "Accepted"},
                new AssetRequest { AssetId = 4, RequestedEmployeeId = 4, ApprovedByEmployeeId = 2, Status = "Accepted"},
                new AssetRequest { AssetId = 5, RequestedEmployeeId = 5, ApprovedByEmployeeId = 2, Status = "New"},

                new AssetRequest { AssetId = 6, RequestedEmployeeId = 1, ApprovedByEmployeeId = 2, Status = "New"},
                new AssetRequest { AssetId = 1, RequestedEmployeeId = 2, ApprovedByEmployeeId = 2, Status = "New"},
            };
        }
        public IEnumerable<AssetIssue> GetAssetAssetIssueList()
        {
            return new List<AssetIssue>
            {
                new AssetIssue { AssetId = 1, RaisedByEmployeeId = 1, Status = "New" },
                new AssetIssue { AssetId = 2, RaisedByEmployeeId = 2, Status = "New" },
                new AssetIssue { AssetId = 3, RaisedByEmployeeId = 3, Status = "Resolved" },
                new AssetIssue { AssetId = 4, RaisedByEmployeeId = 4, Status = "Resolved" },
                new AssetIssue { AssetId = 5, RaisedByEmployeeId = 5, Status = "New" },

                new AssetIssue { AssetId = 6, RaisedByEmployeeId = 1, Status = "New" },
                new AssetIssue { AssetId = 7, RaisedByEmployeeId = 2, Status = "New" },
            };
        }
        public IEnumerable<UserProfileCRUDViewModel> GetUserProfileList()
        {
            return new List<UserProfileCRUDViewModel>
            {
                new UserProfileCRUDViewModel { FirstName = "Employee 5", LastName = "User", Email = "Employee5@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U1.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 4", LastName = "User", Email = "Employee4@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U2.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 3", LastName = "User", Email = "Employee3@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U3.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 2", LastName = "User", Email = "Employee2@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U4.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Employee 1", LastName = "User", Email = "Employee1@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U5.png", Address = "California", Country = "USA", },

                new UserProfileCRUDViewModel { FirstName = "Regular", LastName = "User", Email = "regular@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U6.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Technology", LastName = "User", Email = "tech@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U7.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Finance", LastName = "User", Email = "finance@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U8.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "HR", LastName = "User", Email = "hr@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U9.png", Address = "California", Country = "USA", },
                new UserProfileCRUDViewModel { FirstName = "Accountants", LastName = "User", Email = "accountants@gmail.com", PasswordHash = "123", ConfirmPassword = "123", PhoneNumber= StaticData.RandomDigits(11), ProfilePicture = "/images/UserIcon/U10.png", Address = "California", Country = "USA", },
            };
        }
        public IEnumerable<ManageUserRoles> GetManageRoleList()
        {
            return new List<ManageUserRoles>
            {
                new ManageUserRoles { Name = "Admin", Description = "User Role: New"},
                new ManageUserRoles { Name = "General", Description = "User Role: General"},
            };
        }
        public CompanyInfo GetCompanyInfo()
        {
            return new CompanyInfo
            {
                Name = "XYZ Company Limited",
                Logo = "/upload/company_logo.png",
                Currency = "৳",
                Address = "Dhaka, Bangladesh",
                City = "Dhaka",
                Country = "Bangladesh",
                Phone = "132546789",
                Fax = "9999",
                Website = "www.wyx.com",
            };
        }
        public void SeedTable(ApplicationDbContext _context)
        {
            var _GetAssetStatusList = GetAssetStatusList();
            foreach (var item in _GetAssetStatusList)
            {
                item.CreatedDate = DateTime.Now;
                item.ModifiedDate = DateTime.Now;
                item.CreatedBy = "Admin";
                item.ModifiedBy = "Admin";
                _context.AssetStatus.Add(item);
                _context.SaveChanges();
            }
        }
    }
}
