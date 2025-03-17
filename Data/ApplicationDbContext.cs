using EmployeeLeaveManagement.Models;
using Microsoft.AspNetCore.Identity;
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


            var superAdminRoleId = "1001";
            var adminRoleId = "2001";
            var userRoleId = "3001";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    Id = superAdminRoleId,
                    ConcurrencyStamp = superAdminRoleId
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "User",
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId
                }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            var superAdminId = "101";

            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@email.com",
                Email = "superadmin@email.com",
                NormalizedEmail = "superadmin@email.com".ToUpper(),
                NormalizedUserName = "superadmin@email.com".ToUpper(),
                Id = superAdminId
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superpass@321");

            modelBuilder.Entity<IdentityUser>().HasData(superAdminUser);

            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminId
                }
            };

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);


        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<DropdownSelect> DropdownSelects { get; set; }
        public DbSet<DropdownOption> DropdownOptions { get; set; }
        public DbSet<LeaveApplication> LeaveApplications { get; set; }
        public DbSet<LeaveAdjustmentEntry> LeaveAdjustmentEntries { get; set; }
    }
}
