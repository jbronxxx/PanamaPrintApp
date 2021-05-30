using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PanamaPrintApp.Models;

namespace PanamaPrintApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly CompanyContext _context;

        public OrderController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> это id конкретно выбранной компании</param>
        /// <param name="order"> Новая сущность записи в журнал</param>
        /// <returns>Метод записывает новую запиь выполненной работы в массив Order класса Company.
        /// Таким образом запись привязывается к конкретной компании</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("OrderId,Date,EquipmentName,OrderName,Consumables")] Order order)
        {
            if (id == 0)
                return NotFound();

            var companyId = await _context.Companies.FindAsync(id);

            if (companyId == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                companyId.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // TODO: Изменить метод так, чтобы редактировать мог только Администратор!!!
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Date,EquipmentName,OrderName,Consumables")] Order order)
        {
            if (id != order.OrderId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // TODO: Модифицировать метод так, чтобы при удалении сущности компании, удалялись и ее записи работ!!!
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id) { return _context.Orders.Any(e => e.OrderId == id); }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">ID компании, журнал работ которой нужно вывести</param>
        /// <returns>Метод выводит список работ конкретно выбранной компании</returns>
        [HttpGet]
        public async Task<IActionResult> CompanyOrder(int? id)
        {
            
            if (id == 0)
                return NotFound();

            var companyId = await _context.Companies.FindAsync(id);

            ViewBag.CompanyName = companyId.Name; // Отображает имя компании на странице

            if (companyId == null)
                return NotFound();

            return View(await _context.Orders.Where(o => o.Companies.Contains(companyId)).ToListAsync());
        }
    }
}
