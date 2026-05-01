using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_Project.Models
{
    public class CourseTopic
    {
        [Key]
        public int TopicID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CourseID { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course? Course { get; set; }

        // Navigation property: One Topic has many Materials
        public virtual ICollection<Material>? Materials { get; set; } = new List<Material>();
    }
}
