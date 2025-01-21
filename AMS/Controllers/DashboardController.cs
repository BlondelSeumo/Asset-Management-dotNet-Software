using AMS.Data;
using AMS.Models.DashboardViewModel;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;
        public DashboardController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.Dashboard)]
        public IActionResult Index()
        {
            try
            {
                DashboardSummaryViewModel vm = new DashboardSummaryViewModel();
                var _UserProfile = _context.UserProfile.ToList();
                var _IsInRole = User.IsInRole("Admin");

                vm.TotalUser = _UserProfile.Count();
                vm.TotalActive = _UserProfile.Where(x => x.Cancelled == false).Count();
                vm.TotalInActive = _UserProfile.Where(x => x.Cancelled == true).Count();
                vm.listUserProfile = _UserProfile.Where(x => x.Cancelled == false).OrderByDescending(x => x.CreatedDate).Take(10).ToList();

                var _Asset = _context.Asset.Where(x => x.Cancelled == false).ToList();
                vm.TotalAsset = _Asset.Count();
                vm.TotalAssignedAsset = _Asset.Where(x => x.AssignEmployeeId != 0).Count();
                vm.TotalUnAssignedAsset = _Asset.Where(x => x.AssignEmployeeId == 0).Count();
                vm.listAssetCRUDViewModel = _iCommon.GetGridAssetList(_IsInRole).Take(10).ToList();

                vm.TotalEmployee = _context.UserProfile.Where(x => x.Cancelled == false).Count();
                vm.TotalAssetRequest = _context.AssetRequest.Where(x => x.Cancelled == false).Count();
                vm.TotalIssue = _context.AssetIssue.Where(x => x.Cancelled == false).Count();

                return View(vm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetPieChartData()
        {
            var AssetGroupBy = _context.Asset.Where(x => x.Cancelled == false).GroupBy(p => p.AssetStatus).Select(g => new
            {
                Id = g.Key,
                AssetStatus = g.Count()
            }).ToList();

            var result = (from _AssetGroupBy in AssetGroupBy
                          join _AssetStatus in _context.AssetStatus on _AssetGroupBy.Id equals _AssetStatus.Id
                          select new PieChartViewModel
                          {
                              Name = _AssetStatus.Name,
                              Total = _AssetGroupBy.AssetStatus,
                          }).ToList();
            return new JsonResult(result.ToDictionary(x => x.Name, x => x.Total));
        }
    }
}