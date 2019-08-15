using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public partial class Question
    {
        public Question()
        {
            this.Answer = new HashSet<Answer>();
        }

        public int Importance { get; set; }

        public int QuestionId { get; set; }
        [Display(Name = "Text")]
        public string Text { get; set; }

        public virtual ICollection<Answer> Answer { get; set; }
        public virtual Test Test { get; set; }
    }
}