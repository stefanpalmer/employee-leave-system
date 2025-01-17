using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class LeaveApplication : ApprovalActivity
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        
        [Display(Name = "Number of Days")]
        public int NoOfDays { get; set; }
        
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        
        public string? Description { get; set; }
        public int StatusId { get; set; }
        public DropdownOption Status { get; set; }
    }
}
