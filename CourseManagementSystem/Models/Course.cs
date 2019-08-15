using System;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public partial class Course
    {
        
        public int id { get; set; }

        [Display(Name = "Finished")]
        public bool isFinished { get; set; }    

        [Display(Name = "Course name")]
        public string name { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Publication Date")]
        public System.DateTime PublishDate { get; set; }

        [Display(Name = "Rating")]
        public Nullable<double> Estimation { get; set; }

        [Display(Name = "Number of ratings")]
        public Nullable<int> EstimationCount { get; set; }

        [Display(Name = "Category")]
        public virtual Category Category { get; set; }

        [Display(Name = "Author")]
        public virtual ApplicationUser Author { get; set; }

        [Display(Name = "Status")]
        public bool activated { get; set; }


        [Display(Name = "Certificate Issue")]
        public bool sertificate { get; set; }
 


    }
    public partial class AddCourseModel
    {
        public AddCourseModel(Course course) 
        {
            id = course.id;
            name = course.name;
            description = course.description;
            category_id = course.Category.id;
            sertificate = course.sertificate;
        
        }

        public AddCourseModel()
        {

        }


        public int id { get; set; }

        [Display(Name = "Title")]
        public string name { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Category")]
        public int category_id { get; set; }


        [Display(Name = "Certificate Issue")]
        public bool sertificate { get; set; }





    }


}