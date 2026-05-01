using Final_Project.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class Material
    {
        [Key]
        public int MaterialID { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string MaterialType { get; set; } // Assignment, Quiz, Test

        public int Points { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int? TopicID { get; set; }

        [ForeignKey("TopicID")]
        public virtual CourseTopic? Topic { get; set; }

        [Required]
        public string? CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual ApplicationUser? Creator { get; set; }
    }
}
