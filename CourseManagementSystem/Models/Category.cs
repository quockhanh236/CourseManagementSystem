using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CourseManagementSystem.Models
{
    public partial class Category
    {
        public int id { get; set; }

        public Category()
        {
            this.Course = new HashSet<Course>();
        }

        [Display(Name = "Category")]
        public string Name { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
       
}