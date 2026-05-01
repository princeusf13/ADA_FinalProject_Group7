using Final_Project.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Final_Project.Models
{
    public class TriviaScore
    {
        [Key]
        public int ScoreID { get; set; }

        public string? UserId { get; set; } // Nullable for non-logged in users
        public virtual ApplicationUser? User { get; set; }

        public int Score { get; set; }
        public int TotalQuestions { get; set; } = 10;
        public DateTime DateTaken { get; set; } = DateTime.Now;
        public string Category { get; set; } = "Computers";
    }
}
