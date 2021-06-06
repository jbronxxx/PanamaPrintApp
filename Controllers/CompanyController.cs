using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanamaPrintApp.Models;

namespace PanamaPrintApp.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly CompanyContext _context;

        public CompanyController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public IActionResult Create()
        {
            return View();
        }

        // TODO: Для Администратора
        [HttpPost]
        [Authorize(Roles = "Администратор")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,Name,INN,Adress")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // TODO: Для Администратора
        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                return NotFound();

            return View(company);
        }

        [HttpPost]
        [Authorize(Roles = "Администратор")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompanyId,Name,INN,Adress")] Company company)
        {
            if (id != company.CompanyId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.CompanyId))
                        return NotFound();
                    
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // TODO: Для Администратора
        [HttpGet]
        [Authorize(Roles = "Администратор")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.CompanyId == id);

            if (company == null)
                return NotFound();

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Администратор")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            _context.Companies.Remove(company);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Администратор")]
        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.CompanyId == id);
        }
    }
}
