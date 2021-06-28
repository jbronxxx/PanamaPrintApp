using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using PanamaPrintApp.Models;

namespace PanamaPrintApp.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class PriceController : Controller
    {
        private readonly CompanyContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PriceController(CompanyContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return base.View(await _context.Prices.ToListAsync());
        }

        public IActionResult Create()
        {
            return base.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,Name,Model,ServicePrice")] Price price)
        {
            if (ModelState.IsValid)
            {
                _context.Add(price);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(price);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }
            return View(price);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,Name,Model,ServicePrice")] Price price)
        {
            if (id != price.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(price);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceExists(price.ServiceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(price);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var price = await _context.Prices
                .FirstOrDefaultAsync(m => m.ServiceId == id);
            if (price == null)
            {
                return NotFound();
            }

            return View(price);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var price = await _context.Prices.FindAsync(id);
            _context.Prices.Remove(price);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceExists(int id)
        {
            return _context.Prices.Any(e => e.ServiceId == id);
        }

        [HttpGet]
        public IActionResult View(List<Price> prices = null)
        {
            prices ??= new List<Price>();

            return View(prices);
        }

        [HttpPost]
        public IActionResult ExcelImport(IFormFile file)
        {
            List<Price> prices = new List<Price>();

            Application application = new Application();

            Workbook workbook = application.Workbooks.Open(file.FileName);

            prices.Add(new Price
            {
                Name = workbook.Name
            }); 

            workbook.Close(false, file, null);

            return View(prices);
        }
    }
}
