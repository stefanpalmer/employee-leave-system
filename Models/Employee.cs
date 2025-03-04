using System.ComponentModel;

namespace EmployeeLeaveManagement.Models
{
    public class Employee : UserActivity
    {
        public int Id { get; set; }

        [DisplayName("Employee Number")]
        public string? EmployeeNumber { get; set; }

        [DisplayName("First Name")]
        public string? FirstName { get; set; }

        [DisplayName("Last Name")]
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [DisplayName("Phone Number")]
        public int PhoneNumber { get; set; }

        [DisplayName("E-mail")]
        public string? EmailAddress { get; set; }

        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public string? Address { get; set; }

        public int LeaveDays { get; set; }

        public int DaysRemaining { get; set; }
    }
}
