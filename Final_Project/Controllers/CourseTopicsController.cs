using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Final_Project.Controllers
{
    public class CourseTopicsController : Controller
    {

        private readonly FinalProject_DbContext _context;

        public CourseTopicsController(FinalProject_DbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var topics = await _context.CourseTopics.Include(c => c.Course).ToListAsync();
            return View(topics);
        }

        [HttpPost]
        public async Task<IActionResult> QuickCreate(string name, int courseId)
        {
            if (string.IsNullOrEmpty(name) || courseId == 0) return BadRequest();
            var topic = new CourseTopic { Name = name, CourseID = courseId };
            _context.Add(topic);
            await _context.SaveChangesAsync();
            return Json(new { id = topic.TopicID, name = topic.Name });
        }

        public IActionResult Create()
        {
            // Fetch courses and put them in a SelectList for the dropdown
            ViewBag.CourseID = new SelectList(_context.Courses, "CourseID", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,CourseID")] CourseTopic topic)
        {
            // If the model is valid, it will skip your error-logging block
            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If it fails, we MUST repopulate the dropdown before returning to the view
            ViewBag.CourseID = new SelectList(_context.Courses, "CourseID", "Name", topic.CourseID);
            return View(topic);
        }

        // GET: CourseTopics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var topic = await _context.CourseTopics.FindAsync(id);
            if (topic == null) return NotFound();

            // Fill the dropdown with current courses
            ViewBag.CourseID = new SelectList(_context.Courses, "CourseID", "Name", topic.CourseID);
            return View(topic);
        }

        // POST: CourseTopics/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicID,Name,CourseID")] CourseTopic topic)
        {
            if (id != topic.TopicID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.CourseTopics.Any(e => e.TopicID == topic.TopicID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CourseID = new SelectList(_context.Courses, "CourseID", "Name", topic.CourseID);
            return View(topic);
        }

        // GET: CourseTopics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var topic = await _context.CourseTopics
                .Include(t => t.Course)
                .Include(t => t.Materials) // To show how many items are inside
                .FirstOrDefaultAsync(m => m.TopicID == id);

            if (topic == null) return NotFound();

            return View(topic);
        }

        // POST: CourseTopics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.CourseTopics.FindAsync(id);
            if (topic != null)
            {
                _context.CourseTopics.Remove(topic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> GetTopicsByCourse(int courseId)
        {
            var topics = await _context.CourseTopics
                .Where(t => t.CourseID == courseId)
                .Select(t => new { id = t.TopicID, name = t.Name })
                .ToListAsync();

            return Json(topics);
        }

    }
}
