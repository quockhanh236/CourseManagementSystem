using System.ComponentModel.DataAnnotations;

namespace CourseManagementSystem.Models
{
    public partial class Subscription
    {
        public int SubscriptionId { get; set; }
        [Display(Name = "Subscriber")]

        public ApplicationUser Subscriber { get; set; }

        public virtual Course Course { get; set; }        
    }
}