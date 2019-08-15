namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Questions", "LastLectureId");
            DropColumn("dbo.Questions", "Number");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "Number", c => c.Int(nullable: false));
            AddColumn("dbo.Questions", "LastLectureId", c => c.Int(nullable: false));
        }
    }
}
