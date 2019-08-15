namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Courses", "description", c => c.String());
            AddColumn("dbo.Courses", "activated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Courses", "activated");
            DropColumn("dbo.Courses", "description");
        }
    }
}
