using AMS.Data;
using AMS.Models;
using AMS.Models.SubscriptionRequestViewModel;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class SubscriptionRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public SubscriptionRequestController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.SubscriptionRequest)]
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
                    || obj.Email.ToLower().Contains(searchValue)
                    || obj.TimeZone.ToLower().Contains(searchValue)
                    || obj.CreatedDate.ToString().ToLower().Contains(searchValue)
                    || obj.ModifiedDate.ToString().ToLower().Contains(searchValue)
                    || obj.CreatedBy.ToLower().Contains(searchValue)
                    || obj.ModifiedBy.ToLower().Contains(searchValue));
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

        private IQueryable<SubscriptionRequestCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _SubscriptionRequest in _context.SubscriptionRequest
                        where _SubscriptionRequest.Cancelled == false
                        select new SubscriptionRequestCRUDViewModel
                        {
                            Id = _SubscriptionRequest.Id,
                            Email = _SubscriptionRequest.Email,
                            TimeZone = _SubscriptionRequest.TimeZone,
                            CreatedDate = _SubscriptionRequest.CreatedDate,
                            ModifiedDate = _SubscriptionRequest.ModifiedDate,
                            CreatedBy = _SubscriptionRequest.CreatedBy,
                            ModifiedBy = _SubscriptionRequest.ModifiedBy,
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
            SubscriptionRequestCRUDViewModel vm = await _context.SubscriptionRequest.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
        [HttpGet]
        public async Task<IActionResult> AddEdit(int id)
        {
            SubscriptionRequestCRUDViewModel vm = new SubscriptionRequestCRUDViewModel();
            if (id > 0) vm = await _context.SubscriptionRequest.Where(x => x.Id == id).SingleOrDefaultAsync();
            return PartialView("_AddEdit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(SubscriptionRequestCRUDViewModel vm)
        {
            try
            {
                SubscriptionRequest _SubscriptionRequest = new SubscriptionRequest();
                if (vm.Id > 0)
                {
                    _SubscriptionRequest = await _context.SubscriptionRequest.FindAsync(vm.Id);

                    vm.CreatedDate = _SubscriptionRequest.CreatedDate;
                    vm.CreatedBy = _SubscriptionRequest.CreatedBy;
                    vm.ModifiedDate = DateTime.Now;
                    vm.ModifiedBy = HttpContext.User.Identity.Name;
                    _context.Entry(_SubscriptionRequest).CurrentValues.SetValues(vm);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Subscription Request Updated Successfully. ID: " + _SubscriptionRequest.Id;
                    return new JsonResult(_AlertMessage);
                }
                else
                {
                    _SubscriptionRequest = vm;
                    _SubscriptionRequest.CreatedDate = DateTime.Now;
                    _SubscriptionRequest.ModifiedDate = DateTime.Now;
                    _SubscriptionRequest.CreatedBy = HttpContext.User.Identity.Name;
                    _SubscriptionRequest.ModifiedBy = HttpContext.User.Identity.Name;
                    _context.Add(_SubscriptionRequest);
                    await _context.SaveChangesAsync();

                    var _AlertMessage = "Subscription Request Created Successfully. ID: " + _SubscriptionRequest.Id;
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
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _SubscriptionRequest = await _context.SubscriptionRequest.FindAsync(id);
                _SubscriptionRequest.ModifiedDate = DateTime.Now;
                _SubscriptionRequest.ModifiedBy = HttpContext.User.Identity.Name;
                _SubscriptionRequest.Cancelled = true;

                _context.Update(_SubscriptionRequest);
                await _context.SaveChangesAsync();
                return new JsonResult(_SubscriptionRequest);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> OpenSubscribe()
        {
            return PartialView("_AddFromDashboard");
        }
        [HttpPost]
        public async Task<IActionResult> SaveSubscriptionRequest(string _Email, string _TimeZone)
        {
            try
            {
                var AlredySubscribe = await _context.SubscriptionRequest.Where(x => x.Email == _Email).CountAsync();
                if (AlredySubscribe == 0)
                {
                    SubscriptionRequest _SubscriptionRequest = new();
                    string _UserName = HttpContext.User.Identity.Name;
                    _SubscriptionRequest.Email = _Email;
                    _SubscriptionRequest.TimeZone = _TimeZone;
                    _SubscriptionRequest.CreatedDate = DateTime.Now;
                    _SubscriptionRequest.ModifiedDate = DateTime.Now;
                    _SubscriptionRequest.CreatedBy = _UserName;
                    _SubscriptionRequest.ModifiedBy = _UserName;
                    _context.Add(_SubscriptionRequest);
                    await _context.SaveChangesAsync();
                }

                var _AlertMessage = "Thank you for your Subscription.";
                return new JsonResult(_AlertMessage);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new JsonResult(ex.Message);
                throw ex;
            }
        }
    }
}
