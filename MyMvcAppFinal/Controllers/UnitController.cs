using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using MyMvcAppFinal.Data;
using MyMvcAppFinal.Models;
using MyMvcAppFinal.Models.DTO;
using MyMvcAppFinal.Services;

namespace MyMvcAppFinal.Controllers
{
    public class UnitController : Controller
    {
       
        private IUnitService _unitService;
        public UnitController(UnitContext context, IUnitService unitService)
        {
            _unitService = unitService;
        }

       
        public async Task<IActionResult> Synchronize()
        {
            try
            {
                var path = Path.Combine(Environment.CurrentDirectory, "data.json");
                var jsonData = System.IO.File.ReadAllText(path);
                var localUnits = Newtonsoft.Json.JsonConvert.DeserializeObject<DeserializeUnitListDto>(jsonData)?.Units;
                if (localUnits != null)
                {
                    await _unitService.Synchronize(localUnits);
                }
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            return RedirectToAction("Index");
        }

        // GET: Units
        public async Task<IActionResult> Index()
        {
            return View(await _unitService.Index());
        }

        // GET: Units/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _unitService.Units() == null)
            {
                return NotFound();
            }

            var unit = await _unitService.Details((int)id);
            if (unit == null)
            {
                return NotFound();
            }

            return View(unit);
        }

        // GET: Units/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_unitService.Units(), "Id", "Name");
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
                await _unitService.Create(unit);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_unitService.Units(), "Id", "Name", unit.ParentId);
            return View(unit);
        }

        // GET: Units/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _unitService.Units() == null)
            {
                return NotFound();
            }

            var unit = await _unitService.GetUnitById((int)id);
            if (unit == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_unitService.Units(), "Id", "Name", unit.ParentId);
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
                    await _unitService.Edit(unit);
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
            ViewData["ParentId"] = new SelectList(_unitService.Units(), "Id", "Name", unit.ParentId);
            return View(unit);
        }

        // GET: Units/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _unitService.Units() == null)
            {
                return NotFound();
            }

            var unit = await _unitService.Details((int)id);
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
            if (_unitService.Units() == null)
            {
                return Problem("Entity set 'UnitContext.Units'  is null.");
            }
            var unit = await _unitService.GetUnitById(id);
            if (unit != null)
            {
                await _unitService.DeleteConfirmed(unit);
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool UnitExists(int id)
        {
            return _unitService.UnitExists(id);
        }
    }
}
