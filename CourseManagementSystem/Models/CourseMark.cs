using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace CourseManagementSystem.Models
{
    public class CourseMark:DbContext
    {
        [Key]
        public int id { get; set; }
        public virtual ApplicationUser user { get; set; }
        public virtual Course course { get; set; }
        public int mark { get; set; }
    }
}