using AMS.Data;
using AMS.Models;
using AMS.Services;
using AMS.Models.AssetCategorieViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AssetCategorieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AssetCategorieController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = AMS.Pages.RoleViewModel.AssetCategorie)]
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
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)
                    || obj.ModifiedBy.ToLower().Contains(searchValue)

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

        private IQueryable<AssetCategorieCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _AssetCategorie in _context.AssetCategorie
                        where _AssetCategorie.Cancelled == false
                        select new AssetCategorieCRUDViewModel
                        {
                            Id = _AssetCategorie.Id,
                            Name = _AssetCategorie.Name,
                            Description = _AssetCategorie.Description,
                            CreatedDate = _AssetCategorie.CreatedDate,
                            ModifiedDate = _AssetCategorie.ModifiedDate,
                            CreatedBy = _AssetCategorie.CreatedBy,
                            ModifiedBy = _AssetCategorie.ModifiedBy,

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
            AssetCategorieCRUDViewModel vm = await _context.AssetCategorie.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AssetCategorieCRUDViewModel vm = new AssetCategorieCRUDViewModel();
            if (id > 0) vm = await _context.AssetCategorie.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<JsonResult> AddEdit(AssetCategorieCRUDViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AssetCategorie _AssetCategorie = new();
                    string _UserName = HttpContext.User.Identity.Name;
                    if (vm.Id > 0)
                    {
                        _AssetCategorie = await _context.AssetCategorie.FindAsync(vm.Id);

                        vm.CreatedDate = _AssetCategorie.CreatedDate;
                        vm.CreatedBy = _AssetCategorie.CreatedBy;
                        vm.ModifiedDate = DateTime.Now;
                        vm.ModifiedBy = _UserName;
                        _context.Entry(_AssetCategorie).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();

                        var _AlertMessage = "AssetCategorie Updated Successfully. ID: " + _AssetCategorie.Id;
                        return new JsonResult(_AlertMessage);
                    }
                    else
                    {
                        _AssetCategorie = vm;
                        _AssetCategorie.CreatedDate = DateTime.Now;
                        _AssetCategorie.ModifiedDate = DateTime.Now;
                        _AssetCategorie.CreatedBy = _UserName;
                        _AssetCategorie.ModifiedBy = _UserName;
                        _context.Add(_AssetCategorie);
                        await _context.SaveChangesAsync();

                        var _AlertMessage = "AssetCategorie Created Successfully. ID: " + _AssetCategorie.Id;
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

        [HttpPost]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _AssetCategorie = await _context.AssetCategorie.FindAsync(id);
                _AssetCategorie.ModifiedDate = DateTime.Now;
                _AssetCategorie.ModifiedBy = HttpContext.User.Identity.Name;
                _AssetCategorie.Cancelled = true;

                _context.Update(_AssetCategorie);
                await _context.SaveChangesAsync();
                return new JsonResult(_AssetCategorie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.AssetCategorie.Any(e => e.Id == id);
        }
    }
}
