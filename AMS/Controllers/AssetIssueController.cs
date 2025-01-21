using AMS.Data;
using AMS.Models;
using AMS.Models.AssetIssueViewModel;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AssetIssueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AssetIssueController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.AssetIssue)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetDataTabelData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();

                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnAscDesc = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int resultTotal = 0;

                IQueryable<AssetIssueCRUDViewModel> _GetGridItem = null;
                var _UserEmail = HttpContext.User.Identity.Name;
                var _IsInRole = User.IsInRole("Admin");
                _GetGridItem = _iCommon.GetAssetIssueList(_IsInRole);
                if (_IsInRole)
                {
                    _GetGridItem = _iCommon.GetAssetIssueList(_IsInRole);
                }
                else
                {
                    var _GetLoginEmployeeId = await _iCommon.GetLoginEmployeeId(_UserEmail);
                    _GetGridItem = _iCommon.GetAssetIssueList(_IsInRole).Where(x => x.RaisedByEmployeeId == _GetLoginEmployeeId);
                }

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnAscDesc)))
                {
                    _GetGridItem = _GetGridItem.OrderBy(sortColumn + " " + sortColumnAscDesc);
                }

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower();
                    _GetGridItem = _GetGridItem.Where(obj => obj.Id.ToString().Contains(searchValue)
                    || obj.AssetId.ToString().ToLower().Contains(searchValue)
                    || obj.RaisedByEmployeeId.ToString().ToLower().Contains(searchValue)
                    || obj.IssueDescription.ToLower().Contains(searchValue)
                    || obj.Status.ToLower().Contains(searchValue)
                    || obj.ExpectedFixDate.ToString().ToLower().Contains(searchValue)
                    || obj.ResolvedDate.ToString().ToLower().Contains(searchValue)

                    || obj.CreatedDate.ToString().Contains(searchValue));
                }

                resultTotal = _GetGridItem.Count();

                var result = _GetGridItem.Skip(skip).Take(pageSize).ToList();
                return Json(new { draw = draw, recordsFiltered = resultTotal, recordsTotal = resultTotal, data = result });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            var _IsInRole = User.IsInRole("Admin");
            AssetIssueCRUDViewModel vm = await _iCommon.GetAssetIssueList(_IsInRole).Where(m => m.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AssetIssueCRUDViewModel vm = new AssetIssueCRUDViewModel();
            ViewBag.LoadddlAssetName = new SelectList(_iCommon.GetCommonddlData("Asset"), "Id", "Name");
            ViewBag.LoadddlEmployee = new SelectList(_iCommon.LoadddlEmployee(), "Id", "Name");

            if (id > 0) vm = await _context.AssetIssue.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(AssetIssueCRUDViewModel vm)
        {
            try
            {
                AssetIssue _AssetIssue = new AssetIssue();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _AssetIssue = await _context.AssetIssue.FindAsync(vm.Id);

                    vm.CreatedDate = _AssetIssue.CreatedDate;
                    vm.CreatedBy = _AssetIssue.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_AssetIssue).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Asset Issue Updated Successfully. ID: " + _AssetIssue.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _AssetIssue = vm;
                    _AssetIssue.CreatedDate = DateTime.Now;
                    _AssetIssue.ModifiedDate = DateTime.Now;
                    _AssetIssue.CreatedBy = _UserName;
                    _AssetIssue.ModifiedBy = _UserName;
                    _context.Add(_AssetIssue);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Asset Issue Created Successfully. ID: " + _AssetIssue.Id;
                    return new JsonResult(_AlertMessage);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new JsonResult(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _AssetIssue = await _context.AssetIssue.FindAsync(id);
                _AssetIssue.ModifiedDate = DateTime.Now;
                _AssetIssue.ModifiedBy = HttpContext.User.Identity.Name;
                _AssetIssue.Cancelled = true;

                _context.Update(_AssetIssue);
                await _context.SaveChangesAsync();
                return new JsonResult(_AssetIssue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
