using Final_Project.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
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

    }
}
