using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class Payment
    {
        public int PaymentID { get; set; }
        [DataType(DataType.Currency)]
        public int Amount { get; set; }
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }
        public int EmployeeID { get; set; }
        public virtual Employee Employee { get; set; }
    }
}