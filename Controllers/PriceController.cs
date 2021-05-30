using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanamaPrintApp.Models;

namespace PanamaPrintApp.Controllers
{
    public class PriceController : Controller
    {
        private readonly CompanyContext _context;

        public PriceController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Prices.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,Name,ServicePrice")] Price price)
        {
            if (ModelState.IsValid)
            {
                _context.Add(price);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(price);
        }

        [HttpGet]
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
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,Name,ServicePrice")] Price price)
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
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(price);
        }

        // TODO: Для Администратора
        [HttpGet]
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
    }
}
