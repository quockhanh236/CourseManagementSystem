namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Marks", "Student_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Marks", "Test_Id", "dbo.Tests");
            DropIndex("dbo.Marks", new[] { "Student_Id" });
            DropIndex("dbo.Marks", new[] { "Test_Id" });
            AddColumn("dbo.Marks", "Subscription_SubscriptionId", c => c.Int());
            CreateIndex("dbo.Marks", "Subscription_SubscriptionId");
            AddForeignKey("dbo.Marks", "Subscription_SubscriptionId", "dbo.Subscriptions", "SubscriptionId");
            DropColumn("dbo.Marks", "Student_Id");
            DropColumn("dbo.Marks", "Test_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Marks", "Test_Id", c => c.Int());
            AddColumn("dbo.Marks", "Student_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Marks", "Subscription_SubscriptionId", "dbo.Subscriptions");
            DropIndex("dbo.Marks", new[] { "Subscription_SubscriptionId" });
            DropColumn("dbo.Marks", "Subscription_SubscriptionId");
            CreateIndex("dbo.Marks", "Test_Id");
            CreateIndex("dbo.Marks", "Student_Id");
            AddForeignKey("dbo.Marks", "Test_Id", "dbo.Tests", "Id");
            AddForeignKey("dbo.Marks", "Student_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
