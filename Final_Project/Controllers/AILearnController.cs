using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final_Project.Controllers
{
    [Authorize]
    public class AILearnController : Controller
    {

        private readonly FinalProject_DbContext _context;

        public AILearnController(FinalProject_DbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Tutor()
        {
            ViewBag.CourseID = new SelectList(_context.Courses, "CourseID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogInteraction(int topicId, int? materialId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Json(new { success = false });

            var interaction = new AIInteraction
            {
                UserId = userId,
                TopicID = topicId,
                MaterialID = materialId,
                InteractionDate = DateTime.Now
            };

            _context.AIInteractions.Add(interaction);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

    }
}
