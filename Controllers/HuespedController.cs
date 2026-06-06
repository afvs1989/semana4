using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tarea2_clientes.Data;
using tarea2_clientes.Models;

namespace tarea2_clientes.Controllers
{
    public class HuespedController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HuespedController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Huespedes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var huesped = await _context.Huespedes
                .Include(h => h.Reservas)
                .ThenInclude(r => r.Habitacion)
                .FirstOrDefaultAsync(h => h.HuespedId == id);

            if (huesped == null) return NotFound();

            return View(huesped);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Email,Telefono")] HuespedModel huesped)
        {
            if (ModelState.IsValid)
            {
                _context.Add(huesped);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(huesped);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var huesped = await _context.Huespedes.FindAsync(id);
            if (huesped == null) return NotFound();

            return View(huesped);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HuespedId,Nombre,Apellido,Email,Telefono")] HuespedModel huesped)
        {
            if (id != huesped.HuespedId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(huesped);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HuespedExists(huesped.HuespedId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(huesped);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var huesped = await _context.Huespedes
                .FirstOrDefaultAsync(h => h.HuespedId == id);

            if (huesped == null) return NotFound();

            return View(huesped);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var huesped = await _context.Huespedes.FindAsync(id);
            if (huesped != null)
                _context.Huespedes.Remove(huesped);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HuespedExists(int id)
        {
            return _context.Huespedes.Any(h => h.HuespedId == id);
        }
    }
}
