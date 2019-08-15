namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ResetTokens", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ResetTokens", new[] { "User_Id" });
            AddColumn("dbo.ResetTokens", "UserName", c => c.String());
            DropColumn("dbo.ResetTokens", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ResetTokens", "User_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.ResetTokens", "UserName");
            CreateIndex("dbo.ResetTokens", "User_Id");
            AddForeignKey("dbo.ResetTokens", "User_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
