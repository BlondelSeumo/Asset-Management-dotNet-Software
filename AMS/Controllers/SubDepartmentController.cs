using AMS.Data;
using AMS.Models;
using AMS.Models.SubDepartmentViewModel;
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
    public class SubDepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public SubDepartmentController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.SubDepartment)]
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
                    || obj.DepartmentId.ToString().ToLower().Contains(searchValue)
                    || obj.Name.ToLower().Contains(searchValue)
                    || obj.Description.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)

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

        private IQueryable<SubDepartmentCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _SubDepartment in _context.SubDepartment
                        join _Department in _context.Department on _SubDepartment.DepartmentId equals _Department.Id
                        where _SubDepartment.Cancelled == false
                        select new SubDepartmentCRUDViewModel
                        {
                            Id = _SubDepartment.Id,
                            DepartmentId = _SubDepartment.DepartmentId,
                            DepartmentDisplay = _Department.Name,
                            Name = _SubDepartment.Name,
                            Description = _SubDepartment.Description,
                            CreatedDate = _SubDepartment.CreatedDate,
                            ModifiedDate = _SubDepartment.ModifiedDate,
                            CreatedBy = _SubDepartment.CreatedBy,
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
            SubDepartmentCRUDViewModel vm = await GetGridItem().Where(m => m.Id == id).SingleOrDefaultAsync();
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            SubDepartmentCRUDViewModel vm = new SubDepartmentCRUDViewModel();
            ViewBag._LoadddlDepartment = new SelectList(_iCommon.LoadddlDepartment(), "Id", "Name");
            if (id > 0) vm = await _context.SubDepartment.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<JsonResult> AddEdit(SubDepartmentCRUDViewModel vm)
        {
            try
            {
                SubDepartment _SubDepartment = new SubDepartment();
                string _UserName = HttpContext.User.Identity.Name;

                if (vm.Id > 0)
                {
                    _SubDepartment = await _context.SubDepartment.FindAsync(vm.Id);

                    vm.CreatedDate = _SubDepartment.CreatedDate;
                    vm.CreatedBy = _SubDepartment.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = _UserName;
                    _context.Entry(_SubDepartment).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Sub Department Updated Successfully. ID: " + _SubDepartment.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _SubDepartment = vm;
                    _SubDepartment.CreatedDate = DateTime.Now;
                    _SubDepartment.ModifiedDate = DateTime.Now;
                    _SubDepartment.CreatedBy = _UserName;
                    _SubDepartment.ModifiedBy = _UserName;
                    _context.Add(_SubDepartment);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Sub Department Created Successfully. ID: " + _SubDepartment.Id;
                    return new JsonResult(_AlertMessage);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new JsonResult(ex.Message);
                throw ex;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Int64 id)
        {
            try
            {
                var _SubDepartment = await _context.SubDepartment.FindAsync(id);
                _SubDepartment.ModifiedDate = DateTime.Now;
                _SubDepartment.ModifiedBy = HttpContext.User.Identity.Name;
                _SubDepartment.Cancelled = true;

                _context.Update(_SubDepartment);
                await _context.SaveChangesAsync();
                return new JsonResult(_SubDepartment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.SubDepartment.Any(e => e.Id == id);
        }
    }
}
