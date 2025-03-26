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
    public class LeaveApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeaveApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeaveApplications
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var pending = _context.DropdownOptions
                .Include(x => x.DropdownSelect)
                .Where(y => y.DropdownSelect.SelectProperty == "Leave Approval Status" && y.Option == "Pending")
                .FirstOrDefault();

            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Where(l => l.StatusId == pending!.Id);
            
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApprovedApplications()
        {
            var approved = _context.DropdownOptions
                .Include(x => x.DropdownSelect)
                .Where(y => y.DropdownSelect.SelectProperty == "Leave Approval Status" && y.Option == "Approved")
                .FirstOrDefault();

            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Where(l => l.StatusId == approved!.Id);

            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RejectedApplications()
        {
            var rejected = _context.DropdownOptions
                .Include(x => x.DropdownSelect)
                .Where(y => y.DropdownSelect.SelectProperty == "Leave Approval Status" && y.Option == "Rejected")
                .FirstOrDefault();

            var applicationDbContext = _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .Where(l => l.StatusId == rejected!.Id);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: LeaveApplications/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveApplication leaveApplication)
        {
            var pendingStatus = await _context.DropdownOptions
                .Include(x => x.DropdownSelect)
                .FirstOrDefaultAsync(y => y.Option == "Pending" && y.DropdownSelect.SelectProperty == "Leave Approval Status");
                       
            leaveApplication.CreatedOn = DateTime.Now;
            leaveApplication.CreatedById = "Admin Id";
            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedById = "Admin Id";
            leaveApplication.StatusId = pendingStatus!.Id;

            _context.Add(leaveApplication);
            await _context.SaveChangesAsync();
           
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ApproveLeave(int? id)
        {
            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View(leaveApplication);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ApproveLeave(LeaveApplication application)
        {
            var approved = _context.DropdownOptions
                .Include(x => x.DropdownSelect)
                .Where(y => y.DropdownSelect.SelectProperty == "Leave Approval Status" && y.Option == "Approved")
                .FirstOrDefault();

            var adjustmentOff = _context.DropdownOptions
                .Include(x => x.DropdownSelect)
                .Where(y => y.DropdownSelect.SelectProperty == "Adjustment Type" && y.Option == "Negative")
                .FirstOrDefault();

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == application.Id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedById = "Admin Id";
            leaveApplication.StatusId = approved!.Id;

            _context.Update(leaveApplication);

            var adjustment = new LeaveAdjustmentEntry
            {
                EmployeeId = leaveApplication.EmployeeId,
                NoOfDays = leaveApplication.NoOfDays,
                LeaveAdjustmentDate = DateTime.Now,
                LeaveStartDate = leaveApplication.StartDate,
                LeaveEndDate = leaveApplication.EndDate,
                AdjustmentTypeId = adjustmentOff.Id
            };

            _context.Add(adjustment);

            var employee = await _context.Employees.FindAsync(leaveApplication.EmployeeId);
            employee.DaysRemaining = employee.DaysRemaining - leaveApplication.NoOfDays;
            _context.Update(employee);
            await _context.SaveChangesAsync();

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> RejectLeave(int? id)
        {
            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View(leaveApplication);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> RejectLeave(LeaveApplication application)
        {
            var rejected = _context.DropdownOptions
                .Include(x => x.DropdownSelect)
                .Where(y => y.DropdownSelect.SelectProperty == "Leave Approval Status" && y.Option == "Rejected")
                .FirstOrDefault();
            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == application.Id);

            if (leaveApplication == null)
            {
                return NotFound();
            }

            leaveApplication.ApprovedOn = DateTime.Now;
            leaveApplication.ApprovedById = "Admin Id";
            leaveApplication.StatusId = rejected!.Id;

            _context.Update(leaveApplication);
            await _context.SaveChangesAsync();

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return RedirectToAction(nameof(Index));
        }



        // GET: LeaveApplications/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            return View(leaveApplication);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveApplication leaveApplication)
        {
            if (id != leaveApplication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var pendingStatus = await _context.DropdownOptions.Include(x => x.DropdownSelect).FirstOrDefaultAsync(y => y.Option == "Pending" && y.DropdownSelect.SelectProperty == "Leave Approval Status");

                try
                {
                    leaveApplication.ModifiedOn = DateTime.Now;
                    leaveApplication.ModifiedById = "Admin Id";
                    leaveApplication.StatusId = pendingStatus!.Id;
                    _context.Update(leaveApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveApplicationExists(leaveApplication.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", leaveApplication.EmployeeId);
            ViewData["StatusId"] = new SelectList(_context.DropdownOptions, "Id", "Option", leaveApplication.StatusId);
            return View(leaveApplication);
        }

        // GET: LeaveApplications/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveApplication = await _context.LeaveApplications
                .Include(l => l.Employee)
                .Include(l => l.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveApplication == null)
            {
                return NotFound();
            }

            return View(leaveApplication);
        }

        // POST: LeaveApplications/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveApplication = await _context.LeaveApplications.FindAsync(id);
            if (leaveApplication != null)
            {
                _context.LeaveApplications.Remove(leaveApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveApplicationExists(int id)
        {
            return _context.LeaveApplications.Any(e => e.Id == id);
        }
    }
}
