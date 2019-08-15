using System;
using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public partial class Answer
    {

        public int Id { get; set; }
        public Nullable<int> QuestionId { get; set; }
        [Display(Name = "Тext")]
        public string Text { get; set; }
        public bool IsTrue { get; set; }

        public virtual Question Question { get; set; }
    }
}