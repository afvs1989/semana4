using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tarea2_clientes.Data;
using tarea2_clientes.Models;

namespace tarea2_clientes.Controllers
{
    public class HabitacionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HabitacionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Habitaciones.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var habitacion = await _context.Habitaciones
                .Include(h => h.Reservas)
                .ThenInclude(r => r.Huesped)
                .FirstOrDefaultAsync(h => h.HabitacionId == id);

            if (habitacion == null) return NotFound();

            return View(habitacion);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numero,Tipo,PrecioNoche,Capacidad")] HabitacionModel habitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(habitacion);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null) return NotFound();

            return View(habitacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HabitacionId,Numero,Tipo,PrecioNoche,Capacidad")] HabitacionModel habitacion)
        {
            if (id != habitacion.HabitacionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habitacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitacionExists(habitacion.HabitacionId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(habitacion);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(h => h.HabitacionId == id);

            if (habitacion == null) return NotFound();

            return View(habitacion);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion != null)
                _context.Habitaciones.Remove(habitacion);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitacionExists(int id)
        {
            return _context.Habitaciones.Any(h => h.HabitacionId == id);
        }
    }
}
