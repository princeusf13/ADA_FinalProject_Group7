using Final_Project.Areas.Identity.Data;
using Final_Project.Data;
using Final_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Final_Project.Controllers
{
    public class MaterialsController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FinalProject_DbContext _context;
        public MaterialsController(FinalProject_DbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int? courseFilter, int page = 1)
        {
            int pageSize = 10;
            var query = _context.Materials
                .Include(m => m.Topic)
                .ThenInclude(t => t.Course)
                .AsQueryable();

            // 1. Apply Filtering
            if (courseFilter.HasValue)
            {
                query = query.Where(m => m.Topic.CourseID == courseFilter);
            }

            // 2. Pagination Logic
            var totalItems = await query.CountAsync();
            var materials = await query
                .OrderByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 3. Prepare View Data
            ViewBag.Courses = new SelectList(_context.Courses, "CourseID", "Name", courseFilter);
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.SelectedCourse = courseFilter;

            return View(materials);
        }

        public IActionResult Create()
        {
            // Populate dropdowns
            ViewBag.TopicID = new SelectList(_context.CourseTopics, "TopicID", "Name");
            // We also need CourseID for the "Filter" or "Quick Add" logic
            ViewBag.CourseID = new SelectList(_context.Courses, "CourseID", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material)
        {
            // Auto-assign the logged-in Professor's ID
            material.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            material.CreatedAt = DateTime.Now;

            ModelState.Remove("CreatedByUserId");

            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TopicID = new SelectList(_context.CourseTopics, "TopicID", "Name", material.TopicID);
            return View(material);
        }


        // GET: Materials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var material = await _context.Materials
                .Include(m => m.Topic)
                .ThenInclude(t => t.Course)
                .FirstOrDefaultAsync(m => m.MaterialID == id);

            if (material == null) return NotFound();

            return View(material);
        }

        // POST: Materials/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaterialID,Description,Points,DueDate")] Material material)
        {
            if (id != material.MaterialID) return NotFound();

            // We need to keep the original values for fields not in the form
            var existingMaterial = await _context.Materials.AsNoTracking().FirstOrDefaultAsync(m => m.MaterialID == id);
            if (existingMaterial == null) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Re-attach fields we don't want changed
                    material.Title = existingMaterial.Title;
                    material.TopicID = existingMaterial.TopicID;
                    material.CreatedByUserId = existingMaterial.CreatedByUserId;
                    material.CreatedAt = existingMaterial.CreatedAt;
                    material.MaterialType = existingMaterial.MaterialType;

                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Materials.Any(e => e.MaterialID == material.MaterialID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: Materials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var material = await _context.Materials
                .Include(m => m.Topic)
                .ThenInclude(t => t.Course)
                .FirstOrDefaultAsync(m => m.MaterialID == id);

            if (material == null) return NotFound();

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
