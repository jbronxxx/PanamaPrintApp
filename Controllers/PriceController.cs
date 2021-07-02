using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanamaPrintApp.Models;
using PanamaPrintApp.Service;

namespace PanamaPrintApp.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class PriceController : Controller
    {
        private readonly CompanyContext _context;
        private readonly IExcelService _excelService;

        public PriceController(CompanyContext context, IExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        public async Task<IActionResult> Index()
        {
            return base.View(await _context.Prices.ToListAsync());
        }

        public IActionResult Create() { return base.View(); }

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
                return NotFound();

            var price = await _context.Prices.FindAsync(id);

            if (price == null)
                return NotFound();

            return View(price);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,Name,Model,ServicePrice")] Price price)
        {
            if (id != price.ServiceId)
                return NotFound();

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
                return NotFound();

            var price = await _context.Prices
                .FirstOrDefaultAsync(m => m.ServiceId == id);

            if (price == null)
                return NotFound();

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
        public IActionResult ExcelView()
        {
            return View(_context.ModelList.ToList());
        }

        // Отображает выбранный файл Excel и добавляет данные в БД
        [HttpPost]
        public IActionResult ExcelView(IFormFile file)
        {
            // Возвращает массив данных из Excel файла
             string path = _excelService.FileCreate(file);

            ModelList result = _excelService.ExcelReader(path);

            _context.Add(result);

            _context.SaveChanges();

            //// Проверка на существование записей в БД
            //if (_context.ModelList.Any(x => x.ModelListName == result.ModelListName))
            //{
            //    RedirectToAction("Index");
            //}
            //else
            //{

            //}

            return View(result);
        }
    }
}
