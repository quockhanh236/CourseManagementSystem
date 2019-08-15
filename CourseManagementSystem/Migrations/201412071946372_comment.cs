namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                        text = c.String(),
                        course_id = c.Int(),
                        previosComment_id = c.Int(),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Courses", t => t.course_id)
                .ForeignKey("dbo.Comments", t => t.previosComment_id)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.course_id)
                .Index(t => t.previosComment_id)
                .Index(t => t.user_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "previosComment_id", "dbo.Comments");
            DropForeignKey("dbo.Comments", "course_id", "dbo.Courses");
            DropIndex("dbo.Comments", new[] { "user_Id" });
            DropIndex("dbo.Comments", new[] { "previosComment_id" });
            DropIndex("dbo.Comments", new[] { "course_id" });
            DropTable("dbo.Comments");
        }
    }
}
