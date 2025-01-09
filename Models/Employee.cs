namespace EmployeeLeaveManagement.Models
{
    public class Employee : UserActivity
    {
        public int Id { get; set; }

        public string? EmployeeNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public int PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string? Address { get; set; }
    }
}
