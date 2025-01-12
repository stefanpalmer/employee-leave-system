using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagement.Models
{
    public class DropdownSelect
    {
        [Key]
        public int Id { get; set; }
        public string SelectProperty { get; set; }
    }
}
