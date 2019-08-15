using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public class Mark
    {
        public int id { get; set; }

        public virtual ApplicationUser Student { get; set; }

        public virtual Test Test { get; set; }

        [Display(Name = "Rating")]
        public int Value { get; set; }
    }
}