using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project.Controllers
{
    public class CoursesController : Controller
    {

        private readonly FinalProject_DbContext _context;

        public CoursesController(FinalProject_DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> QuickCreate(string name, string description)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest("Name is required");

            var course = new Course
            {
                Name = name,
                Description = description,
                CreatedAt = DateTime.Now
            };

            _context.Add(course);
            await _context.SaveChangesAsync();
            return Json(new { id = course.CourseID, name = course.Name });
        }

        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                course.CreatedAt = DateTime.Now;
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }


        // GET: Edit Course
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses.FindAsync(id);
            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Name,Description,CreatedAt")] Course course)
        {
            if (id != course.CourseID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Courses.Any(e => e.CourseID == course.CourseID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Delete Confirmation
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _context.Courses
                .Include(c => c.CourseTopics) // Include children to show counts in warning
                .FirstOrDefaultAsync(m => m.CourseID == id);

            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
