namespace EmployeeLeaveManagement.Models
{
    public class LeaveAdjustmentEntry
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int NoOfDays { get; set; }
        public DateTime LeaveAdjustmentDate { get; set; }
        public DateTime? LeaveStartDate { get; set; }
        public DateTime? LeaveEndDate { get; set; }
        public int AdjustmentTypeId { get; set; }
        public DropdownOption AdjustmentType { get; set; }

    }
}
