namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Marks", "Subscription_SubscriptionId", "dbo.Subscriptions");
            DropForeignKey("dbo.Subscriptions", "CourseId", "dbo.Courses");
            DropIndex("dbo.Marks", new[] { "Subscription_SubscriptionId" });
            DropIndex("dbo.Subscriptions", new[] { "CourseId" });
            RenameColumn(table: "dbo.Subscriptions", name: "CourseId", newName: "Course_id");
            AddColumn("dbo.Marks", "Student_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Marks", "Test_Id", c => c.Int());
            AlterColumn("dbo.Subscriptions", "Course_id", c => c.Int());
            CreateIndex("dbo.Marks", "Student_Id");
            CreateIndex("dbo.Marks", "Test_Id");
            CreateIndex("dbo.Subscriptions", "Course_id");
            AddForeignKey("dbo.Marks", "Student_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Marks", "Test_Id", "dbo.Tests", "Id");
            AddForeignKey("dbo.Subscriptions", "Course_id", "dbo.Courses", "id");
            DropColumn("dbo.Marks", "Subscription_SubscriptionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Marks", "Subscription_SubscriptionId", c => c.Int());
            DropForeignKey("dbo.Subscriptions", "Course_id", "dbo.Courses");
            DropForeignKey("dbo.Marks", "Test_Id", "dbo.Tests");
            DropForeignKey("dbo.Marks", "Student_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Subscriptions", new[] { "Course_id" });
            DropIndex("dbo.Marks", new[] { "Test_Id" });
            DropIndex("dbo.Marks", new[] { "Student_Id" });
            AlterColumn("dbo.Subscriptions", "Course_id", c => c.Int(nullable: false));
            DropColumn("dbo.Marks", "Test_Id");
            DropColumn("dbo.Marks", "Student_Id");
            RenameColumn(table: "dbo.Subscriptions", name: "Course_id", newName: "CourseId");
            CreateIndex("dbo.Subscriptions", "CourseId");
            CreateIndex("dbo.Marks", "Subscription_SubscriptionId");
            AddForeignKey("dbo.Subscriptions", "CourseId", "dbo.Courses", "id", cascadeDelete: true);
            AddForeignKey("dbo.Marks", "Subscription_SubscriptionId", "dbo.Subscriptions", "SubscriptionId");
        }
    }
}
