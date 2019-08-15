namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        PublishDate = c.DateTime(nullable: false),
                        Author_Id = c.String(maxLength: 128),
                        Category_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.Author_Id)
                .ForeignKey("dbo.Categories", t => t.Category_id)
                .Index(t => t.Author_Id)
                .Index(t => t.Category_id);
            
            AddColumn("dbo.Categories", "Name", c => c.String());
            DropColumn("dbo.Categories", "CategoryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "CategoryName", c => c.String());
            DropForeignKey("dbo.Courses", "Category_id", "dbo.Categories");
            DropForeignKey("dbo.Courses", "Author_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Courses", new[] { "Category_id" });
            DropIndex("dbo.Courses", new[] { "Author_Id" });
            DropColumn("dbo.Categories", "Name");
            DropTable("dbo.Courses");
        }
    }
}
