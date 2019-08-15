using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public partial class Lecture
    {
        public int Id { get; set; }
        public int CourseId { get; set; }

        [Display(Name = "Lecture title")]
        public string Name { get; set; }

        [Display(Name = "Content")]
        public string Text { get; set; }
        public int Number { get; set; }

        public virtual Course Course { get; set; }
    }
}