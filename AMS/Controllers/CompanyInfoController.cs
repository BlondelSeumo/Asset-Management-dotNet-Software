using AMS.Data;
using AMS.Models;
using AMS.Models.CompanyInfoViewModel;
using AMS.Pages;
using AMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace InvenToryPlus.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class CompanyInfoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _iCommon;

        public CompanyInfoController(ApplicationDbContext context, ICommon iCommon)
        {
            _context = context;
            _iCommon = iCommon;
        }

        [Authorize(Roles = RoleViewModel.CompanyInfo)]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            CompanyInfoCRUDViewModel vm = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);
            return View(vm);
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            CompanyInfoCRUDViewModel vm = await _context.CompanyInfo.FirstOrDefaultAsync(m => m.Id == 1);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompanyInfoCRUDViewModel vm)
        {
            try
            {
                CompanyInfo _CompanyInfo = new CompanyInfo();
                _CompanyInfo = await _context.CompanyInfo.FindAsync(vm.Id);
                if (vm.CompanyLogo != null)
                    vm.Logo = "/upload/" + _iCommon.UploadedFile(vm.CompanyLogo);
                vm.ModifiedDate = DateTime.Now;
                vm.ModifiedBy = HttpContext.User.Identity.Name;
                _context.Entry(_CompanyInfo).CurrentValues.SetValues(vm);
                await _context.SaveChangesAsync();
                TempData["successAlert"] = "Company Info Updated Successfully. Company Name: " + _CompanyInfo.Name;
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {              
                throw ex;
            }
        }

        private bool IsExists(long id)
        {
            return _context.CompanyInfo.Any(e => e.Id == id);
        }
    }
}

