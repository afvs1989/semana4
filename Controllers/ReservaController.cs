using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tarea2_clientes.Data;
using tarea2_clientes.Models;

namespace tarea2_clientes.Controllers
{
    public class ReservaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Huesped)
                .Include(r => r.Habitacion)
                .Include(r => r.Empleado)
                .OrderByDescending(r => r.FechaInicio)
                .ToListAsync();

            return View(reservas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Huesped)
                .Include(r => r.Habitacion)
                .Include(r => r.Empleado)
                .Include(r => r.Pagos)
                .FirstOrDefaultAsync(r => r.ReservaId == id);

            if (reserva == null) return NotFound();

            return View(reserva);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View(new ReservaModel
            {
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(1),
                Estado = "Pendiente"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FechaInicio,FechaFin,Estado,Total,HuespedId,HabitacionId,EmpleadoId")] ReservaModel reserva)
        {
            if (reserva.FechaFin <= reserva.FechaInicio)
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la de inicio");

            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(reserva.HuespedId, reserva.HabitacionId, reserva.EmpleadoId);
            return View(reserva);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            PopulateDropdowns(reserva.HuespedId, reserva.HabitacionId, reserva.EmpleadoId);
            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReservaId,FechaInicio,FechaFin,Estado,Total,HuespedId,HabitacionId,EmpleadoId")] ReservaModel reserva)
        {
            if (id != reserva.ReservaId) return NotFound();

            if (reserva.FechaFin <= reserva.FechaInicio)
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la de inicio");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.ReservaId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(reserva.HuespedId, reserva.HabitacionId, reserva.EmpleadoId);
            return View(reserva);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Huesped)
                .Include(r => r.Habitacion)
                .Include(r => r.Empleado)
                .FirstOrDefaultAsync(r => r.ReservaId == id);

            if (reserva == null) return NotFound();

            return View(reserva);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
                _context.Reservas.Remove(reserva);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(r => r.ReservaId == id);
        }

        private void PopulateDropdowns(int? huespedId = null, int? habitacionId = null, int? empleadoId = null)
        {
            var huespedes = _context.Huespedes.OrderBy(h => h.Apellido)
                .AsEnumerable()
                .Select(h => new { h.HuespedId, NombreCompleto = $"{h.Nombre} {h.Apellido}" });

            var habitaciones = _context.Habitaciones.OrderBy(h => h.Numero)
                .AsEnumerable()
                .Select(h => new { h.HabitacionId, Descripcion = $"#{h.Numero} - {h.Tipo}" });

            var empleados = _context.Empleados.OrderBy(e => e.Nombre)
                .AsEnumerable()
                .Select(e => new { e.EmpleadoId, Descripcion = $"{e.Nombre} ({e.Cargo})" });

            ViewBag.HuespedId = new SelectList(huespedes, "HuespedId", "NombreCompleto", huespedId);
            ViewBag.HabitacionId = new SelectList(habitaciones, "HabitacionId", "Descripcion", habitacionId);
            ViewBag.EmpleadoId = new SelectList(empleados, "EmpleadoId", "Descripcion", empleadoId);
        }
    }
}
