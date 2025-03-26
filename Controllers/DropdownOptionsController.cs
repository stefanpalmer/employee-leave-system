using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeLeaveManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DropdownOptionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DropdownOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DropdownOptions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DropdownOptions.Include(d => d.DropdownSelect);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DropdownOptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dropdownOption = await _context.DropdownOptions
                .Include(d => d.DropdownSelect)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dropdownOption == null)
            {
                return NotFound();
            }

            return View(dropdownOption);
        }

        // GET: DropdownOptions/Create
        public IActionResult Create()
        {
            ViewData["DropdownSelectId"] = new SelectList(_context.DropdownSelects, "Id", "SelectProperty");
            return View();
        }

        // POST: DropdownOptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DropdownOption dropdownOption)
        {
            
            _context.Add(dropdownOption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            ViewData["DropdownSelectId"] = new SelectList(_context.DropdownSelects, "Id", "SelectProperty", dropdownOption.DropdownSelectId);
            return View(dropdownOption);
        }

        // GET: DropdownOptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dropdownOption = await _context.DropdownOptions.FindAsync(id);
            if (dropdownOption == null)
            {
                return NotFound();
            }
            ViewData["DropdownSelectId"] = new SelectList(_context.DropdownSelects, "Id", "Id", dropdownOption.DropdownSelectId);
            return View(dropdownOption);
        }

        // POST: DropdownOptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DropdownSelectId,Option,SequenceId")] DropdownOption dropdownOption)
        {
            if (id != dropdownOption.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dropdownOption);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DropdownOptionExists(dropdownOption.Id))
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
            ViewData["DropdownSelectId"] = new SelectList(_context.DropdownSelects, "Id", "Id", dropdownOption.DropdownSelectId);
            return View(dropdownOption);
        }

        // GET: DropdownOptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dropdownOption = await _context.DropdownOptions
                .Include(d => d.DropdownSelect)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dropdownOption == null)
            {
                return NotFound();
            }

            return View(dropdownOption);
        }

        // POST: DropdownOptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dropdownOption = await _context.DropdownOptions.FindAsync(id);
            if (dropdownOption != null)
            {
                _context.DropdownOptions.Remove(dropdownOption);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DropdownOptionExists(int id)
        {
            return _context.DropdownOptions.Any(e => e.Id == id);
        }
    }
}
