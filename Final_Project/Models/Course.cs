using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property: One Course has many Topics
        public virtual ICollection<CourseTopic> CourseTopics { get; set; } = new List<CourseTopic>();
    }
}
