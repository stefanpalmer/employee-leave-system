using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;

namespace EmployeeLeaveManagement.Controllers
{
    public class DropdownSelectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DropdownSelectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DropdownSelects
        public async Task<IActionResult> Index()
        {
            return View(await _context.DropdownSelects.ToListAsync());
        }

        // GET: DropdownSelects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dropdownSelect = await _context.DropdownSelects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dropdownSelect == null)
            {
                return NotFound();
            }

            return View(dropdownSelect);
        }

        // GET: DropdownSelects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DropdownSelects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SelectProperty")] DropdownSelect dropdownSelect)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dropdownSelect);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dropdownSelect);
        }

        // GET: DropdownSelects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dropdownSelect = await _context.DropdownSelects.FindAsync(id);
            if (dropdownSelect == null)
            {
                return NotFound();
            }
            return View(dropdownSelect);
        }

        // POST: DropdownSelects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SelectProperty")] DropdownSelect dropdownSelect)
        {
            if (id != dropdownSelect.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dropdownSelect);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DropdownSelectExists(dropdownSelect.Id))
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
            return View(dropdownSelect);
        }

        // GET: DropdownSelects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dropdownSelect = await _context.DropdownSelects
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dropdownSelect == null)
            {
                return NotFound();
            }

            return View(dropdownSelect);
        }

        // POST: DropdownSelects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dropdownSelect = await _context.DropdownSelects.FindAsync(id);
            if (dropdownSelect != null)
            {
                _context.DropdownSelects.Remove(dropdownSelect);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DropdownSelectExists(int id)
        {
            return _context.DropdownSelects.Any(e => e.Id == id);
        }
    }
}
