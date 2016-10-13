using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }
        public string MarkAttendance { get; set; }
        public string AttendanceStatus { get; }
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }
}