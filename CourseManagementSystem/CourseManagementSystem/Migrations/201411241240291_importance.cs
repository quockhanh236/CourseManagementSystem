namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class importance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Importance", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Importance");
        }
    }
}
