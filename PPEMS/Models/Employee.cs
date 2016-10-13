using System.ComponentModel.DataAnnotations;

namespace PPEMS.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string ContactNo { get; set; }
        [StringLength(255, MinimumLength = 20, ErrorMessage ="Max length 255 and min length is 20")]
        public string Address { get; set; }
    }
}