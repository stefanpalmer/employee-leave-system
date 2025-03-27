using EmployeeLeaveManagement.Data;
using EmployeeLeaveManagement.Interfaces;

namespace EmployeeLeaveManagement.Services
{
    public class LeaveDaysService : ILeaveDaysService
    {
        private readonly ApplicationDbContext _context;

        public LeaveDaysService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void ResetLeaveDays()
        {
            DateTime today = DateTime.Today;

            if (today.Month == 1 && today.Day == 1)
            {
                var employees = _context.Employees.ToList();

                foreach (var employee in employees)
                {
                    employee.DaysRemaining = 11;
                }
            }

            else if (today.Month == 7 && today.Day == 1)
            {
                var employees = _context.Employees.ToList();

                foreach (var employee in employees)
                {
                    employee.DaysRemaining = 10;
                }
            }

            _context.SaveChanges();
        }
    }
}
