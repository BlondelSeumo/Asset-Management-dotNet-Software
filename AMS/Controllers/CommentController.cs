using AMS.Data;
using AMS.Models;
using AMS.Models.CommentViewModel;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace AMS.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public CommentController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = Pages.RoleViewModel.Comment)]
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

                var _GetGridItem = _iCommon.GetCommentList();
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
                    || obj.Message.ToLower().Contains(searchValue)
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
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null) return NotFound();
            CommentCRUDViewModel vm = await _context.Comment.FirstOrDefaultAsync(m => m.Id == id);
            if (vm == null) return NotFound();
            return PartialView("_Details", vm);
        }


        [HttpPost]
        public async Task<JsonResult> AddNewComment(CommentCRUDViewModel vm)
        {
            try
            {
                Comment _Comment = vm;
                _Comment.CreatedDate = DateTime.Now;
                _Comment.ModifiedDate = DateTime.Now;
                _Comment.CreatedBy = HttpContext.User.Identity.Name;
                _Comment.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Add(_Comment);
                await _context.SaveChangesAsync();
                var message = "Comment added successfully. ID: " + _Comment.Id;
                return new JsonResult(message);
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
                var _Comment = await _context.Comment.FindAsync(id);
                _Comment.ModifiedDate = DateTime.Now;
                _Comment.ModifiedBy = HttpContext.User.Identity.Name;
                _Comment.Cancelled = true;
                _Comment.IsDeleted = true;
                _context.Update(_Comment);
                await _context.SaveChangesAsync();

                var message = "Comment deleted successfully. ID: " + _Comment.Id;
                return new JsonResult(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
