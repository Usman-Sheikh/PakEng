using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PPEMS.Models
{
    public class PPEMSContext : IdentityDbContext<ApplicationUser>
    {
        public PPEMSContext()
            : base("PPEMSContext", throwIfV1Schema: false)
        {
        }
        public DbSet<Challan> Challans { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoiceItems> InvoiceItems { get; set; }
        public DbSet<Letter> Letters { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<QuotationDetails> QuotationsDetails { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<ChallanItems> ChallanItems { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public static PPEMSContext Create()
        {
            return new PPEMSContext();
        }
    }
}