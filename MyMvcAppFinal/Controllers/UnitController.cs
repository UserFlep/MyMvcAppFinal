using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyMvcAppFinal.Data;
using MyMvcAppFinal.Models;

namespace MyMvcAppFinal.Controllers
{
    public class UnitController : Controller
    {
        private readonly UnitContext _context;

        public UnitController(UnitContext context)
        {
            _context = context;
        }

        // GET: Units
        public async Task<IActionResult> Index()
        {
            var unitContext = _context.Units.Include(u => u.Parent);
            return View(await unitContext.ToListAsync());
        }

        // GET: Units/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Units == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // GET: Units/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Units, "Id", "Name");
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ParentId")] Unit unit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(unit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Units, "Id", "Name", unit.ParentId);
            return View(unit);
        }

        // GET: Units/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Units == null)
            {
                return NotFound();
            }

            var unit = await _context.Units.FindAsync(id);
            if (unit == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Units, "Id", "Name", unit.ParentId);
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ParentId")] Unit unit)
        {
            if (id != unit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(unit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UnitExists(unit.Id))
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
            ViewData["ParentId"] = new SelectList(_context.Units, "Id", "Name", unit.ParentId);
            return View(unit);
        }

        // GET: Units/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Units == null)
            {
                return NotFound();
            }

            var unit = await _context.Units
                .Include(u => u.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Units == null)
            {
                return Problem("Entity set 'UnitContext.Units'  is null.");
            }
            var unit = await _context.Units.FindAsync(id);
            if (unit != null)
            {
                _context.Units.Remove(unit);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id)
        {
          return _context.Units.Any(e => e.Id == id);
        }
    }
}
