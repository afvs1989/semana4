using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tarea2_clientes.Data;
using tarea2_clientes.Models;

namespace tarea2_clientes.Controllers
{
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pagos = await _context.Pagos
                .Include(p => p.Reserva)
                .ThenInclude(r => r!.Huesped)
                .OrderByDescending(p => p.FechaPago)
                .ToListAsync();

            return View(pagos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pago = await _context.Pagos
                .Include(p => p.Reserva)
                .ThenInclude(r => r!.Huesped)
                .Include(p => p.Reserva)
                .ThenInclude(r => r!.Habitacion)
                .FirstOrDefaultAsync(p => p.PagoId == id);

            if (pago == null) return NotFound();

            return View(pago);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            return View(new PagoModel
            {
                FechaPago = DateTime.Now,
                Estado = "Completado"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Monto,Metodo,FechaPago,Estado,ReservaId")] PagoModel pago)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(pago.ReservaId);
            return View(pago);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();

            PopulateDropdowns(pago.ReservaId);
            return View(pago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PagoId,Monto,Metodo,FechaPago,Estado,ReservaId")] PagoModel pago)
        {
            if (id != pago.PagoId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PagoExists(pago.PagoId))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateDropdowns(pago.ReservaId);
            return View(pago);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pago = await _context.Pagos
                .Include(p => p.Reserva)
                .ThenInclude(r => r!.Huesped)
                .FirstOrDefaultAsync(p => p.PagoId == id);

            if (pago == null) return NotFound();

            return View(pago);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago != null)
                _context.Pagos.Remove(pago);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(p => p.PagoId == id);
        }

        private void PopulateDropdowns(int? reservaId = null)
        {
            var reservas = _context.Reservas
                .Include(r => r.Huesped)
                .Include(r => r.Habitacion)
                .OrderByDescending(r => r.FechaInicio)
                .AsEnumerable()
                .Select(r => new
                {
                    r.ReservaId,
                    Descripcion = $"#{r.ReservaId} - {r.Huesped?.Nombre} {r.Huesped?.Apellido} (Hab. {r.Habitacion?.Numero})"
                });

            ViewBag.ReservaId = new SelectList(reservas, "ReservaId", "Descripcion", reservaId);
        }
    }
}
