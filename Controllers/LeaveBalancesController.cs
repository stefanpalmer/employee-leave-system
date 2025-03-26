using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeaveBalancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveBalancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var results = await _context.Employees.ToListAsync();

            return View(results);
        }

        [HttpGet]
        public IActionResult AdjustLeaveBalance(int id)
        {
            LeaveAdjustmentEntry leaveAdjustment = new();
            leaveAdjustment.EmployeeId = id;
            ViewData["AdjustmentTypeId"] = new SelectList(_context.DropdownOptions
                .Include(y=>y.DropdownSelect)
                .Where(y=>y.DropdownSelect.SelectProperty=="Adjustment Type"), "Id", "Option");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", id);
            return View(leaveAdjustment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdjustLeaveBalance(LeaveAdjustmentEntry leaveAdjustmentEntry)
        {
            
            var adjustmentType = _context.DropdownOptions
            .Include(x => x.DropdownSelect)
            .Where(y => y.DropdownSelect.SelectProperty == "Adjustment Type" && y.Id == leaveAdjustmentEntry.AdjustmentTypeId)
            .FirstOrDefault();

            leaveAdjustmentEntry.Id = 0;
            _context.Add(leaveAdjustmentEntry);

            var employee = await _context.Employees.FindAsync(leaveAdjustmentEntry.EmployeeId);

            if (adjustmentType.Option == "Positive")
            {
                employee.DaysRemaining = employee.DaysRemaining + leaveAdjustmentEntry.NoOfDays;
            }
            else
            {
                employee.DaysRemaining = employee.DaysRemaining - leaveAdjustmentEntry.NoOfDays;
            }
                
            _context.Update(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            ViewData["AdjustmentTypeId"] = new SelectList(_context.DropdownOptions, "Id", "Option", leaveAdjustmentEntry.AdjustmentTypeId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FirstName", leaveAdjustmentEntry.EmployeeId);
            return View(leaveAdjustmentEntry);
        }
        
    }
}
