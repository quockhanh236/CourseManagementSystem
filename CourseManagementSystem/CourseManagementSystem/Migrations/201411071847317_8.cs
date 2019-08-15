namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ResetTokens",
                c => new
                    {
                        Token = c.String(nullable: false, maxLength: 128),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Token)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ResetTokens", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ResetTokens", new[] { "User_Id" });
            DropTable("dbo.ResetTokens");
        }
    }
}
