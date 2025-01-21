using AMS.Data;
using AMS.Models;
using AMS.Services;
using AMS.Models.AssetStatusViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AssetStatusController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AssetStatusController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = AMS.Pages.RoleViewModel.AssetStatus)]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetDataTabelData()
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

                var _GetGridItem = GetGridItem();
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
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    
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

        private IQueryable<AssetStatusCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _AssetStatus in _context.AssetStatus
                        where _AssetStatus.Cancelled == false
                        select new AssetStatusCRUDViewModel
                        {
                            Id = _AssetStatus.Id,
                            Name = _AssetStatus.Name,
                            Description = _AssetStatus.Description,
                            CreatedDate = _AssetStatus.CreatedDate,
                            ModifiedDate = _AssetStatus.ModifiedDate,
                            CreatedBy = _AssetStatus.CreatedBy,
                            ModifiedBy = _AssetStatus.ModifiedBy,

                        }).OrderByDescending(x => x.Id);
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
            AssetStatusCRUDViewModel vm = await _context.AssetStatus.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AssetStatusCRUDViewModel vm = new AssetStatusCRUDViewModel();
            if (id > 0) vm = await _context.AssetStatus.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(AssetStatusCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        AssetStatus _AssetStatus = new();
                        if (vm.Id > 0)
                        {
                            _AssetStatus = await _context.AssetStatus.FindAsync(vm.Id);

                            vm.CreatedDate = _AssetStatus.CreatedDate;
                            vm.CreatedBy = _AssetStatus.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_AssetStatus).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "AssetStatus Updated Successfully. ID: " + _AssetStatus.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _AssetStatus = vm;
                            _AssetStatus.CreatedDate = DateTime.Now;
                            _AssetStatus.ModifiedDate = DateTime.Now;
                            _AssetStatus.CreatedBy = HttpContext.User.Identity.Name;
                            _AssetStatus.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_AssetStatus);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "AssetStatus Created Successfully. ID: " + _AssetStatus.Id;
                            return new JsonResult(_AlertMessage);
                        }
                    }
                    return new JsonResult("Operation failed.");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    return new JsonResult(ex.Message);
                    throw ex;
                }
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _AssetStatus = await _context.AssetStatus.FindAsync(id);
                _AssetStatus.ModifiedDate = DateTime.Now;
                _AssetStatus.ModifiedBy = HttpContext.User.Identity.Name;
                _AssetStatus.Cancelled = true;

                _context.Update(_AssetStatus);
                await _context.SaveChangesAsync();
                return new JsonResult(_AssetStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.AssetStatus.Any(e => e.Id == id);
        }
    }
}
