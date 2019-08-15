namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        Student_Id = c.String(maxLength: 128),
                        Test_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.Student_Id)
                .ForeignKey("dbo.Tests", t => t.Test_Id)
                .Index(t => t.Student_Id)
                .Index(t => t.Test_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Marks", "Test_Id", "dbo.Tests");
            DropForeignKey("dbo.Marks", "Student_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Marks", new[] { "Test_Id" });
            DropIndex("dbo.Marks", new[] { "Student_Id" });
            DropTable("dbo.Marks");
        }
    }
}
