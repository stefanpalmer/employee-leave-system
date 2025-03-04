using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<LeaveApplication>()
                .HasOne(f => f.Status)
                .WithMany()
                .HasForeignKey(f => f.StatusId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<DropdownSelect> DropdownSelects { get; set; }
        public DbSet<DropdownOption> DropdownOptions { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<LeaveAdjustmentEntry> LeaveAdjustmentEntries { get; set; }
    }
}
