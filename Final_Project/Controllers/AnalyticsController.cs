using Final_Project.Areas.Identity.Data;
using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class AnalyticsController : Controller
    {

        private readonly FinalProject_DbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnalyticsController(FinalProject_DbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var model = new AnalyticsViewModel();

            // 1. Leaderboard (Top 10)
            model.Leaderboard = await _context.TriviaScores
                .Include(s => s.User)
                .GroupBy(s => new { s.User.FirstName, s.User.LastName })
                .Select(g => new LeaderboardEntry
                {
                    Name = g.Key.FirstName + " " + g.Key.LastName,
                    TotalScore = g.Sum(s => s.Score)
                })
                .OrderByDescending(x => x.TotalScore)
                .Take(10)
                .ToListAsync();

            // 2. Topics per Course
            model.TopicsPerCourse = await _context.Courses
                .Select(c => new ChartDataPoint
                {
                    Label = c.Name,
                    Value = c.CourseTopics.Count()
                }).ToListAsync();

            // 3. Materials Distribution (Assignments vs Quizzes)
            model.MaterialsByType = await _context.Materials
                .GroupBy(m => m.MaterialType)
                .Select(g => new ChartDataPoint
                {
                    Label = g.Key,
                    Value = g.Count()
                }).ToListAsync();

            // 4. Personal Performance (Last 7)
            var personalAttempts = await _context.TriviaScores
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.DateTaken)
                .Take(7)
                .Reverse() // Oldest to newest for the line chart
                .ToListAsync();

            model.PersonalScores = personalAttempts.Select(a => a.Score).ToList();
            model.PersonalDates = personalAttempts.Select(a => a.DateTaken.ToString("MM/dd")).ToList();

            // 1. Top 3 Most Interacted Topics (Topic - Course format)
            model.TopTopics = await _context.AIInteractions
                .Include(i => i.Topic)
                .ThenInclude(t => t.Course)
                .GroupBy(i => new { i.Topic.Name, CourseName = i.Topic.Course.Name })
                .Select(g => new ChartDataPoint
                {
                    Label = g.Key.Name + " (" + g.Key.CourseName + ")",
                    Value = g.Count()
                })
                .OrderByDescending(x => x.Value)
                .Take(3)
                .ToListAsync();

            // 2. Personal AI Use in Last 7 Days
            var last7Days = Enumerable.Range(0, 7)
                .Select(i => DateTime.Today.AddDays(-i))
                .Reverse()
                .ToList();

            var userInteractions = await _context.AIInteractions
                .Where(i => i.UserId == userId && i.InteractionDate >= DateTime.Today.AddDays(-6))
                .ToListAsync();

            model.UsageDays = last7Days.Select(d => d.ToString("ddd")).ToList(); // "Mon", "Tue", etc.
            model.PersonalUsageCounts = last7Days
                .Select(d => userInteractions.Count(i => i.InteractionDate.Date == d.Date))
                .ToList();

            return View(model);
        }
    }
}
