using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentMVC.Data;
using StudentMVC.Models;

namespace StudentMVC.Controllers;

public class CourseController : Controller
{
    private readonly AppDbContext _context;

    public CourseController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Course
    public async Task<IActionResult> Index()
    {
        return View(await _context.Courses.Include(c => c.Students).ToListAsync());
    }

    // GET: Course/Add
    public IActionResult Add()
    {
        return View();
    }

    // POST: Course/Add
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add([Bind("Id,Name")] Course course)
    {
        if (ModelState.IsValid)
        {
            _context.Add(course);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Kursus berhasil ditambahkan";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = "Data Kursus tidak valid";
        return View(course);
    }

    // GET: Course/Edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "Data Siswa tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }

        var course = await _context.Courses.FindAsync(id);
        if (course == null)
        {
            TempData["ErrorMessage"] = "Data Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }
        return View(course);
    }

    // POST: Course/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Course course)
    {
        if (id != course.Id)
        {
            TempData["ErrorMessage"] = "ID Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.Id))
                {
                    TempData["ErrorMessage"] = "ID Kursus tidak ditemukan";
                    return RedirectToAction("Index", "Course");
                }
                else
                {
                    throw;
                }
            }
            TempData["SuccessMessage"] = "Data Kursus berhasil diubah";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = "Data Kursus tidak valid";
        return View(course);
    }

    // GET: Course/Manage/{id}
    public async Task<IActionResult> Manage(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "ID Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }

        var course = await _context
            .Courses.Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
        {
            TempData["ErrorMessage"] = "Data Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }

        var allStudents = await _context.Students.ToListAsync();

        var vm = new Models.CourseParticipant
        {
            Course = course,
            AllStudents = allStudents,
            SelectedStudentIds = course.Students.Select(s => s.Id).ToList(),
        };
        return View(vm);
    }

    // POST: Course/UpdateParticipants/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateParticipants(int id, List<int>? selectedStudentIds)
    {
        var course = await _context
            .Courses.Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null)
        {
            TempData["ErrorMessage"] = "Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }

        course.Students.Clear();

        if (selectedStudentIds != null && selectedStudentIds.Any())
        {
            var students = await _context
                .Students.Where(s => selectedStudentIds.Contains(s.Id))
                .ToListAsync();

            foreach (var s in students)
            {
                course.Students.Add(s);
            }
        }

        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Data Peserta Kursus berhasil disimpan";
        return RedirectToAction(nameof(Details), new { id });
    }

    // GET: Course/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "ID Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }

        var course = await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);
        if (course == null)
        {
            TempData["ErrorMessage"] = "Data Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }
        return View(course);
    }

    // POST: Course/Delete/{id}
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
        TempData["SuccessMessage"] = "Data Kursus berhasil dihapus";
        return RedirectToAction(nameof(Index));
    }

    // GET: Course/Details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "ID Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }

        var course = await _context
            .Courses.Include(c => c.Students)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (course == null)
        {
            TempData["ErrorMessage"] = "Data Kursus tidak ditemukan";
            return RedirectToAction("Index", "Course");
        }
        return View(course);
    }

    private bool CourseExists(int id)
    {
        return _context.Courses.Any(e => e.Id == id);
    }
}
