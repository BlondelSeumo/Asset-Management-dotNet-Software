using AMS.Data;
using AMS.Helpers;
using AMS.Models;
using AMS.Models.AssetViewModel;
using AMS.Models.ReportViewModel;
using AMS.Pages;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public ReportController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = RoleViewModel.AssetStatusReport)]
        [HttpGet]
        public IActionResult AssetStatusReport()
        {
            ViewBag.StartDate = MinMaxDate().Item1;
            ViewBag.EndDate = MinMaxDate().Item2;
            ManageReportViewModel vm = new ManageReportViewModel();
            vm.AssetStatusViewModel = GetAssetStatusData(null, null, false);
            vm.CompanyInfoCRUDViewModel = _iCommon.GetCompanyInfo();
            return View(vm);
        }
        [HttpGet]
        public IActionResult AssetStatusReportByDate(string StartDate, string EndDate)
        {
            ManageReportViewModel vm = new ManageReportViewModel();
            vm.AssetStatusViewModel = GetAssetStatusData(StartDate, EndDate, true);
            vm.CompanyInfoCRUDViewModel = _iCommon.GetCompanyInfo();
            return View("AssetStatusReport", vm);
        }

        [Authorize(Roles = RoleViewModel.PrintBarcode)]
        [HttpGet]
        public IActionResult PrintBarcode()
        {
            ScannerCodeViewModel _BarcodeViewModel = new();
            var result = _iCommon.GetBarcodeList().ToList();
            
            _BarcodeViewModel.listScannerCodeViewModel = result.Where(f => f.Id % 2 != 0).ToList();
            _BarcodeViewModel.sublistScannerCodeViewModel = result.Where(f => f.Id % 2 == 0).ToList();

            return View(_BarcodeViewModel);
        }
        [Authorize(Roles = RoleViewModel.PrintQRcode)]
        [HttpGet]
        public IActionResult PrintQRcode()
        {
            ScannerCodeViewModel vm = new();
            var result = _iCommon.GetQRcodeList().ToList();
            vm.listScannerCodeViewModel = result.Where(f => f.Id % 2 != 0).ToList();
            vm.sublistScannerCodeViewModel = result.Where(f => f.Id % 2 == 0).ToList();
            return View(vm);
        }


        private AssetStatusViewModel GetAssetStatusData(string StartDate, string EndDate, bool IsRangeData)
        {
            if (StartDate == null)
                StartDate = DateTime.Today.ToString();
            if (EndDate == null)
                EndDate = DateTime.Today.ToString();
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            List<Asset> _AssetList = new List<Asset>();
            if (!IsRangeData)
                _AssetList = _context.Asset.Where(x => x.Cancelled == false).ToList();
            else
            {
                _AssetList = (from obj in _context.Asset
                              where (obj.Cancelled == false) &&
                              (obj.CreatedDate >= Convert.ToDateTime(StartDate))
                              && (obj.CreatedDate <= Convert.ToDateTime(EndDate).AddDays(1))
                              select obj).ToList();
            }
            AssetStatusViewModel _vm = new AssetStatusViewModel
            {
                New = _AssetList.Where(x => x.AssetStatus == AssetStatusValue.New).Count(),
                InUse = _AssetList.Where(x => x.AssetStatus == AssetStatusValue.InUse).Count(),
                Available = _AssetList.Where(x => x.AssetStatus == AssetStatusValue.Available).Count(),
                Damage = _AssetList.Where(x => x.AssetStatus == AssetStatusValue.Damage).Count(),

                Return = _AssetList.Where(x => x.AssetStatus == AssetStatusValue.Return).Count(),
                Expired = _AssetList.Where(x => x.AssetStatus == AssetStatusValue.Expired).Count(),
                RequiredLicenseUpdate = _AssetList.Where(x => x.AssetStatus == AssetStatusValue.RequiredLicenseUpdate).Count(),
                Miscellaneous = _AssetList.Where(x => x.AssetStatus == AssetStatusValue.Miscellaneous).Count(),

                Total = _AssetList.Count()
            };
            return _vm;
        }

        [Authorize(Roles = RoleViewModel.AssetAllocationReport)]
        [HttpGet]
        public IActionResult AssetAllocationReport()
        {
            ViewBag.StartDate = MinMaxDate().Item1;
            ViewBag.EndDate = MinMaxDate().Item2;
            ManageReportViewModel vm = new ManageReportViewModel();
            vm.AllocationViewModel = GetAssetAllocationReportData(null, null, false);
            vm.CompanyInfoCRUDViewModel = _iCommon.GetCompanyInfo();
            return View(vm);
        }
        [HttpGet]
        public IActionResult AssetAllocationReportByDate(string StartDate, string EndDate)
        {
            ManageReportViewModel vm = new ManageReportViewModel();
            vm.AllocationViewModel = GetAssetAllocationReportData(StartDate, EndDate, true);
            vm.CompanyInfoCRUDViewModel = _iCommon.GetCompanyInfo();
            return View("AssetAllocationReport", vm);
        }
        
        private List<AllocationViewModel> GetAssetAllocationReportData(string StartDate, string EndDate, bool IsRangeData)
        {
            if (StartDate == null)
                StartDate = DateTime.Today.ToString();
            if (EndDate == null)
                EndDate = DateTime.Today.ToString();
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            List<Asset> _AssetList = new List<Asset>();
            if (!IsRangeData)
                _AssetList = _context.Asset.Where(x => x.Cancelled == false).ToList();
            else
            {
                _AssetList = (from obj in _context.Asset
                              where (obj.Cancelled == false) &&
                                  (obj.CreatedDate >= Convert.ToDateTime(StartDate))
                                  && (obj.CreatedDate <= Convert.ToDateTime(EndDate).AddDays(1))
                              select obj).ToList();
            }


            var AssetGroupBy = _AssetList.GroupBy(p => p.AssignEmployeeId).Select(g => new
            {
                EmployeeId = g.Key,
                TotalAssigned = g.Count()
            }).ToList();

            var result = (from _AssetGroupBy in AssetGroupBy
                          join _Employee in _context.UserProfile on _AssetGroupBy.EmployeeId equals _Employee.UserProfileId
                          select new AllocationViewModel
                          {
                              EmployeeId = _AssetGroupBy.EmployeeId,
                              UserName = _Employee.FirstName + "  " + _Employee.LastName,
                              TotalAssigned = _AssetGroupBy.TotalAssigned
                          }).ToList();

            var _UnassignedAsset = AssetGroupBy.Where(x => x.EmployeeId == 0).SingleOrDefault();
            if (_UnassignedAsset != null)
            {
                AllocationViewModel vm = new AllocationViewModel();
                vm.UserName = "Unassigned";
                vm.TotalAssigned = _UnassignedAsset.TotalAssigned;
                result.Add(vm);
            }

            return result;
        }

        private Tuple<DateTime, DateTime> MinMaxDate()
        {
            DateTime MinDate;
            DateTime MaxDate;
            if (_context.Asset.Count() == 0)
            {
                MinDate = DateTime.Today;
                MaxDate = DateTime.Today;
            }
            else
            {
                MinDate = _context.Asset.OrderByDescending(t => t.CreatedDate).Last().CreatedDate;
                MaxDate = _context.Asset.OrderByDescending(t => t.CreatedDate).First().CreatedDate;
            }
            return Tuple.Create(MinDate, MaxDate);
        }
    }
}
