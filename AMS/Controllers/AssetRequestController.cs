using AMS.Data;
using AMS.Models;
using AMS.Models.AssetRequestViewModel;
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
    public class AssetRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AssetRequestController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.AssetRequest)]
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

                IQueryable<AssetRequestCRUDViewModel> _GetGridItem = null;
                var _UserEmail = HttpContext.User.Identity.Name;
                var _IsInRole = User.IsInRole("Admin");
                _GetGridItem = _iCommon.GetAssetRequestList(_IsInRole);
                if (_IsInRole)
                {
                    _GetGridItem = _iCommon.GetAssetRequestList(_IsInRole);
                }
                else
                {
                    var _GetLoginEmployeeId = await _iCommon.GetLoginEmployeeId(_UserEmail);
                    _GetGridItem = _iCommon.GetAssetRequestList(_IsInRole).Where(x => x.RequestedEmployeeId == _GetLoginEmployeeId);
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
                    || obj.RequestedEmployeeId.ToString().ToLower().Contains(searchValue)
                    || obj.ApprovedByEmployeeId.ToString().ToLower().Contains(searchValue)
                    || obj.RequestDetails.ToLower().Contains(searchValue)
                    || obj.Status.ToLower().Contains(searchValue)
                    || obj.RequestDate.ToString().ToLower().Contains(searchValue)

                    || obj.RequestDate.ToString().Contains(searchValue)
                    || obj.ReceiveDate.ToString().Contains(searchValue));
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
            AssetRequestCRUDViewModel vm = await _iCommon.GetAssetRequestList(_IsInRole).Where(m => m.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AssetRequestCRUDViewModel vm = new AssetRequestCRUDViewModel();
            ViewBag.LoadddlAssetName = new SelectList(_iCommon.GetCommonddlData("Asset"), "Id", "Name");
            ViewBag.LoadddlEmployee = new SelectList(_iCommon.LoadddlEmployee(), "Id", "Name");

            if (id > 0) vm = await _context.AssetRequest.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(AssetRequestCRUDViewModel vm)
        {
            try
            {
                AssetRequest _AssetRequest = new AssetRequest();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _AssetRequest = await _context.AssetRequest.FindAsync(vm.Id);

                    vm.CreatedDate = _AssetRequest.CreatedDate;
                    vm.CreatedBy = _AssetRequest.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_AssetRequest).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Asset Request Updated Successfully. ID: " + _AssetRequest.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _AssetRequest = vm;
                    _AssetRequest.CreatedDate = DateTime.Now;
                    _AssetRequest.ModifiedDate = DateTime.Now;
                    _AssetRequest.CreatedBy = _UserName;
                    _AssetRequest.ModifiedBy = _UserName;
                    _context.Add(_AssetRequest);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Asset Request Created Successfully. ID: " + _AssetRequest.Id;
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
                var _AssetRequest = await _context.AssetRequest.FindAsync(id);
                _AssetRequest.ModifiedDate = DateTime.Now;
                _AssetRequest.ModifiedBy = HttpContext.User.Identity.Name;
                _AssetRequest.Cancelled = true;

                _context.Update(_AssetRequest);
                await _context.SaveChangesAsync();
                return new JsonResult(_AssetRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
