namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseMarks",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        mark = c.Int(nullable: false),
                        course_id = c.Int(),
                        user_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Courses", t => t.course_id)
                .ForeignKey("dbo.AspNetUsers", t => t.user_Id)
                .Index(t => t.course_id)
                .Index(t => t.user_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CourseMarks", "user_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.CourseMarks", "course_id", "dbo.Courses");
            DropIndex("dbo.CourseMarks", new[] { "user_Id" });
            DropIndex("dbo.CourseMarks", new[] { "course_id" });
            DropTable("dbo.CourseMarks");
        }
    }
}
