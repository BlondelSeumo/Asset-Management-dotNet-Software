using AMS.Data;
using AMS.Models;
using AMS.Models.DesignationViewModel;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class DesignationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public DesignationController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.Designation)]
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

        private IQueryable<DesignationCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _Designation in _context.Designation
                        where _Designation.Cancelled == false
                        select new DesignationCRUDViewModel
                        {
                            Id = _Designation.Id,
                            Name = _Designation.Name,
                            Description = _Designation.Description,
                            CreatedDate = _Designation.CreatedDate,
                            ModifiedDate = _Designation.ModifiedDate,
                            CreatedBy = _Designation.CreatedBy,
                            ModifiedBy = _Designation.ModifiedBy,

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
            DesignationCRUDViewModel vm = await _context.Designation.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            DesignationCRUDViewModel vm = new DesignationCRUDViewModel();
            if (id > 0) vm = await _context.Designation.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(DesignationCRUDViewModel vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Designation _Designation = new Designation();
                        if (vm.Id > 0)
                        {
                            _Designation = await _context.Designation.FindAsync(vm.Id);

                            vm.CreatedDate = _Designation.CreatedDate;
                            vm.CreatedBy = _Designation.CreatedBy;
                            vm.ModifiedDate = DateTime.Now;
                            vm.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Entry(_Designation).CurrentValues.SetValues(vm);
                            await _context.SaveChangesAsync();

                            var _AlertMessage = "Designation Updated Successfully. ID: " + _Designation.Id;
                            return new JsonResult(_AlertMessage);
                        }
                        else
                        {
                            _Designation = vm;
                            _Designation.CreatedDate = DateTime.Now;
                            _Designation.ModifiedDate = DateTime.Now;
                            _Designation.CreatedBy = HttpContext.User.Identity.Name;
                            _Designation.ModifiedBy = HttpContext.User.Identity.Name;
                            _context.Add(_Designation);
                            await _context.SaveChangesAsync();
                            var _AlertMessage = "Designation Created Successfully. ID: " + _Designation.Id;
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

        [HttpDelete]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _Designation = await _context.Designation.FindAsync(id);
                _Designation.ModifiedDate = DateTime.Now;
                _Designation.ModifiedBy = HttpContext.User.Identity.Name;
                _Designation.Cancelled = true;

                _context.Update(_Designation);
                await _context.SaveChangesAsync();
                return new JsonResult(_Designation);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
