using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public partial class Test
    {

        public int Id { get; set; }
        public Test()
        {
            this.Question = new HashSet<Question>();
        }

        [Display(Name = "Lecture name")]
        public int LastLectureId { get; set; }
        [Display(Name = "Room")]
        public int Number { get; set; }

        public virtual Lecture Lecture { get; set; }
        public virtual ICollection<Question> Question { get; set; }
    }
}