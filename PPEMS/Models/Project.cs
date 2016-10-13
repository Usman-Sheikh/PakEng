using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Client { get; set; }
        [Required]
        public string ManagerName { get; set; }
        [Required]
        public string ReferenceNo { get; set; }
        [Required]
        public string ClientAddress { get; set; }
        [Required]
        public string ContactNo { get; set; }
        [Required]
        public string ClientSpkoesman { get; set; }
        [Required]
        public string ClientCompanyName { get; set; }
    }
}