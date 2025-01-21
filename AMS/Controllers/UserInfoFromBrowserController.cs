using AMS.Data;
using AMS.Models.UserInfoFromBrowserViewModel;
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
    public class UserInfoFromBrowserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public UserInfoFromBrowserController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.SuperAdmin)]
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
                    || obj.BrowserUniqueID.ToLower().Contains(searchValue)
                    || obj.Lat.ToLower().Contains(searchValue)
                    || obj.Long.ToLower().Contains(searchValue)
                    || obj.TimeZone.ToLower().Contains(searchValue)
                    || obj.BrowserMajor.ToLower().Contains(searchValue)
                    || obj.BrowserName.ToLower().Contains(searchValue)

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

        private IQueryable<UserInfoFromBrowserCRUDViewModel> GetGridItem()
        {
            try
            {
                return (from _UserInfoFromBrowser in _context.UserInfoFromBrowser
                        where _UserInfoFromBrowser.Cancelled == false
                        select new UserInfoFromBrowserCRUDViewModel
                        {
                            Id = _UserInfoFromBrowser.Id,
                            BrowserUniqueID = _UserInfoFromBrowser.BrowserUniqueID,
                            Lat = _UserInfoFromBrowser.Lat,
                            Long = _UserInfoFromBrowser.Long,
                            TimeZone = _UserInfoFromBrowser.TimeZone,
                            BrowserMajor = _UserInfoFromBrowser.BrowserMajor,
                            BrowserName = _UserInfoFromBrowser.BrowserName,
                            CreatedDate = _UserInfoFromBrowser.CreatedDate,

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
            UserInfoFromBrowserCRUDViewModel vm = await _context.UserInfoFromBrowser.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }
    }
}
