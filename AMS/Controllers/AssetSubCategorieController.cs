using AMS.Data;
using AMS.Models;
using AMS.Models.AssetSubCategorieViewModel;
using AMS.Models.CommonViewModel;
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
    public class AssetSubCategorieController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public AssetSubCategorieController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.AssetSubCategorie)]
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
                    || obj.AssetCategorieDisplay.ToLower().Contains(searchValue)
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

        private IQueryable<AssetSubCategorieCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _AssetSubCategorie in _context.AssetSubCategorie
                        join _AssetCategorie in _context.AssetCategorie on _AssetSubCategorie.AssetCategorieId equals _AssetCategorie.Id
                        where _AssetSubCategorie.Cancelled == false
                        select new AssetSubCategorieCRUDViewModel
                        {
                            Id = _AssetSubCategorie.Id,
                            AssetCategorieId = _AssetSubCategorie.AssetCategorieId,
                            AssetCategorieDisplay = _AssetCategorie.Name,
                            Name = _AssetSubCategorie.Name,
                            Description = _AssetSubCategorie.Description,
                            CreatedDate = _AssetSubCategorie.CreatedDate,
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
            AssetSubCategorieCRUDViewModel vm = await GetGridItem().Where(m => m.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            AssetSubCategorieCRUDViewModel vm = new AssetSubCategorieCRUDViewModel();
            ViewBag._LoadddlAssetCategorie = new SelectList(_iCommon.LoadddlAssetCategorie(), "Id", "Name");
            if (id > 0) vm = await _context.AssetSubCategorie.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(AssetSubCategorieCRUDViewModel vm)
        {
            JsonResultViewModel _JsonResultViewModel = new();
            try
            {
                AssetSubCategorie _AssetSubCategorie = new();
                string _UserName = HttpContext.User.Identity.Name;
                if (vm.Id > 0)
                {
                    _AssetSubCategorie = await _context.AssetSubCategorie.FindAsync(vm.Id);

                    vm.CreatedDate = _AssetSubCategorie.CreatedDate;
                    vm.CreatedBy = _AssetSubCategorie.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_AssetSubCategorie).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    _JsonResultViewModel.AlertMessage = "Asset Sub Categorie Updated Successfully. ID: " + _AssetSubCategorie.Id;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
                else
                {
                    _AssetSubCategorie = vm;
                    _AssetSubCategorie.CreatedDate = DateTime.Now;
                    _AssetSubCategorie.ModifiedDate = DateTime.Now;
                    _AssetSubCategorie.CreatedBy = _UserName;
                    _AssetSubCategorie.ModifiedBy = _UserName;
                    _context.Add(_AssetSubCategorie);
                    await _context.SaveChangesAsync();

                    _JsonResultViewModel.AlertMessage = "Asset Sub Categorie Created Successfully. ID: " + _AssetSubCategorie.Id;
                    _JsonResultViewModel.IsSuccess = true;
                    return new JsonResult(_JsonResultViewModel);
                }
            }
            catch (Exception ex)
            {
                _JsonResultViewModel.IsSuccess = false;
                _JsonResultViewModel.AlertMessage = ex.Message;
                return new JsonResult(_JsonResultViewModel);
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _AssetSubCategorie = await _context.AssetSubCategorie.FindAsync(id);
                _AssetSubCategorie.ModifiedDate = DateTime.Now;
                _AssetSubCategorie.ModifiedBy = HttpContext.User.Identity.Name;
                _AssetSubCategorie.Cancelled = true;

                _context.Update(_AssetSubCategorie);
                await _context.SaveChangesAsync();
                return new JsonResult(_AssetSubCategorie);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.AssetSubCategorie.Any(e => e.Id == id);
        }
    }
}
