using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (ModelState.IsValid)
            {
                string filePath = Path.GetTempFileName();

                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);

                    //stream.Flush();
                }
            }
            
            var prices = GetPriceList(file.FileName);

            return View(prices);
        }

        private List<Price> GetPriceList(string fName)
        {
            List<Price> prices = new List<Price>();

            //var fileName = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fName;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = System.IO.File.Open(fName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        prices.Add(new Price()
                        {
                            Name = reader.GetValue(1).ToString(),
                            ServicePrice = reader.GetValue(2).ToString()
                        });
                    }
                }
            }

            return prices;
        }
    }
}
