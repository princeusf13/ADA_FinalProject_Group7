using Final_Project.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class AIInteraction
    {
        [Key]
        public int InteractionID { get; set; }

        
        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        // Relationship to the Course Topic 
        // (Since Topic is linked to Course, we only need TopicID to find both)
        [Required]
        public int TopicID { get; set; }
        public virtual CourseTopic? Topic { get; set; }

        public DateTime InteractionDate { get; set; } = DateTime.Now;

        // Optional: We can track the specific Material/Assignment that triggered the session
        public int? MaterialID { get; set; }
        public virtual Material? Material { get; set; }
    }
}
