using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class DropdownOption
    {
        [Key]
        public int Id { get; set; }
        public int DropdownSelectId { get; set; }
        public DropdownSelect DropdownSelect { get; set; }
        public string Option { get; set; }
        public int? SequenceId { get; set; }
    }
}
