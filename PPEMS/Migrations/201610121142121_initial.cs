namespace PPEMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendances",
                c => new
                    {
                        AttendanceID = c.Int(nullable: false, identity: true),
                        AttendanceDate = c.DateTime(nullable: false),
                        MarkAttendance = c.String(),
                        EmployeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AttendanceID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Designation = c.String(),
                        ContactNo = c.String(),
                        Address = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.EmployeeID);
            
            CreateTable(
                "dbo.ChallanItems",
                c => new
                    {
                        ChallanItemsID = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Unit = c.String(),
                        Description = c.String(),
                        ChallanID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChallanItemsID)
                .ForeignKey("dbo.Challans", t => t.ChallanID, cascadeDelete: true)
                .Index(t => t.ChallanID);
            
            CreateTable(
                "dbo.Challans",
                c => new
                    {
                        ChallanID = c.Int(nullable: false, identity: true),
                        ChallanNo = c.String(),
                        Date = c.DateTime(nullable: false),
                        Body = c.String(),
                        Note = c.String(),
                        Remarks = c.String(),
                        SentBy = c.String(),
                        RecievedBy = c.String(),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChallanID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Client = c.String(nullable: false),
                        ManagerName = c.String(nullable: false),
                        ReferenceNo = c.String(nullable: false),
                        ClientAddress = c.String(nullable: false),
                        ContactNo = c.String(nullable: false),
                        ClientSpkoesman = c.String(nullable: false),
                        ClientCompanyName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectID);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false, identity: true),
                        Attention = c.String(),
                        Subject = c.String(),
                        BillNo = c.String(),
                        Date = c.DateTime(nullable: false),
                        NTN = c.String(),
                        RefNo = c.String(),
                        Body = c.String(nullable: false),
                        Sender = c.String(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        InvoiceItemsID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Unit = c.String(),
                        Quantity = c.Int(nullable: false),
                        Rate = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        ItemType = c.String(),
                        InvoiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InvoiceItemsID)
                .ForeignKey("dbo.Invoices", t => t.InvoiceID, cascadeDelete: true)
                .Index(t => t.InvoiceID);
            
            CreateTable(
                "dbo.Letters",
                c => new
                    {
                        LetterID = c.Int(nullable: false, identity: true),
                        ReferenceNo = c.String(),
                        Date = c.String(),
                        Attention = c.String(),
                        Subject = c.String(),
                        Body = c.String(),
                        PONo = c.String(),
                        Sender = c.String(),
                        SenderDesignation = c.String(),
                        Attachments = c.String(),
                        Payment = c.String(),
                        DeliveryAt = c.String(),
                        DeliveryTime = c.String(),
                        AcceptedBy = c.String(),
                        ProjectID = c.Int(),
                        VendorID = c.Int(),
                        LetterType = c.String(),
                        NTN = c.String(),
                        Email = c.String(),
                        FileName = c.String(),
                        FilePath = c.String(),
                    })
                .PrimaryKey(t => t.LetterID);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentID = c.Int(nullable: false, identity: true),
                        Amount = c.Int(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PaymentID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .Index(t => t.EmployeeID);
            
            CreateTable(
                "dbo.Quotations",
                c => new
                    {
                        QuotationID = c.Int(nullable: false, identity: true),
                        ReferenceNo = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Attention = c.String(nullable: false),
                        Subject = c.String(nullable: false),
                        Body = c.String(nullable: false),
                        Sender = c.String(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuotationID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.QuotationDetails",
                c => new
                    {
                        QuotationDetailsID = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Unit = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitCost = c.Int(nullable: false),
                        QuantityCost = c.Int(nullable: false),
                        QuotationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.QuotationDetailsID)
                .ForeignKey("dbo.Quotations", t => t.QuotationID, cascadeDelete: true)
                .Index(t => t.QuotationID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        VendorID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ContactNo = c.String(),
                        Address = c.String(),
                        CompanyTitle = c.String(),
                    })
                .PrimaryKey(t => t.VendorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.QuotationDetails", "QuotationID", "dbo.Quotations");
            DropForeignKey("dbo.Quotations", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Payments", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.InvoiceItems", "InvoiceID", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ChallanItems", "ChallanID", "dbo.Challans");
            DropForeignKey("dbo.Challans", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Attendances", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.QuotationDetails", new[] { "QuotationID" });
            DropIndex("dbo.Quotations", new[] { "ProjectID" });
            DropIndex("dbo.Payments", new[] { "EmployeeID" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceID" });
            DropIndex("dbo.Invoices", new[] { "ProjectID" });
            DropIndex("dbo.Challans", new[] { "ProjectID" });
            DropIndex("dbo.ChallanItems", new[] { "ChallanID" });
            DropIndex("dbo.Attendances", new[] { "EmployeeID" });
            DropTable("dbo.Vendors");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.QuotationDetails");
            DropTable("dbo.Quotations");
            DropTable("dbo.Payments");
            DropTable("dbo.Letters");
            DropTable("dbo.InvoiceItems");
            DropTable("dbo.Invoices");
            DropTable("dbo.Projects");
            DropTable("dbo.Challans");
            DropTable("dbo.ChallanItems");
            DropTable("dbo.Employees");
            DropTable("dbo.Attendances");
        }
    }
}
