namespace CourseManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(),
                        Text = c.String(),
                        IsTrue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        QuestionId = c.Int(nullable: false, identity: true),
                        LastLectureId = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Text = c.String(),
                        Test_Id = c.Int(),
                    })
                .PrimaryKey(t => t.QuestionId)
                .ForeignKey("dbo.Tests", t => t.Test_Id)
                .Index(t => t.Test_Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastLectureId = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Lecture_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lectures", t => t.Lecture_Id)
                .Index(t => t.Lecture_Id);
            
            CreateTable(
                "dbo.Lectures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        Name = c.String(),
                        Text = c.String(),
                        Number = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        SubscriptionId = c.Int(nullable: false, identity: true),
                        CourseId = c.Int(nullable: false),
                        Subscriber_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.SubscriptionId)
                .ForeignKey("dbo.Courses", t => t.CourseId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Subscriber_Id)
                .Index(t => t.CourseId)
                .Index(t => t.Subscriber_Id);
            
            AddColumn("dbo.Courses", "Estimation", c => c.Double());
            AddColumn("dbo.Courses", "EstimationCount", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "Subscriber_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Subscriptions", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Questions", "Test_Id", "dbo.Tests");
            DropForeignKey("dbo.Tests", "Lecture_Id", "dbo.Lectures");
            DropForeignKey("dbo.Lectures", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Subscriptions", new[] { "Subscriber_Id" });
            DropIndex("dbo.Subscriptions", new[] { "CourseId" });
            DropIndex("dbo.Questions", new[] { "Test_Id" });
            DropIndex("dbo.Tests", new[] { "Lecture_Id" });
            DropIndex("dbo.Lectures", new[] { "CourseId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropColumn("dbo.Courses", "EstimationCount");
            DropColumn("dbo.Courses", "Estimation");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.Lectures");
            DropTable("dbo.Tests");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
