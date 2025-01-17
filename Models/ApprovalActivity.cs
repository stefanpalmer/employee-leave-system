namespace EmployeeLeaveManagement.Models
{
    public class ApprovalActivity : UserActivity
    {
        public string ApprovedById { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
}
