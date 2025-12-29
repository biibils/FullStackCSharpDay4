using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentMVC.Data;
using StudentMVC.Models;

namespace StudentMVC.Controllers;

public class StudentController : Controller
{
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string searchName)
    {
        var students = await _context
            .Students.Where(s =>
                string.IsNullOrEmpty(searchName) || (s.Name != null && s.Name.Contains(searchName))
            )
            .ToListAsync();
        return View(students);
    }

    // GET: Student/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Student/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Data Siswa berhasil dibuat";
            return RedirectToAction(nameof(Index));
        }
        TempData["ErrorMessage"] = "Data Siswa tidak valid";
        return View(student);
    }

    // GET: Student/Edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "ID Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            TempData["ErrorMessage"] = "Data Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }
        return View(student);
    }

    // POST: Student/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Age")] Student student)
    {
        if (id != student.Id)
        {
            TempData["ErrorMessage"] = "ID Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.Id))
                {
                    TempData["ErrorMessage"] = "ID Siswa tidak ditemukan";
                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    throw;
                }
            }
            TempData["SuccessMessage"] = "Data Siswa berhasil diubah";
            return RedirectToAction(nameof(Index));
        }
        return View(student);
    }

    // GET: Student/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "ID Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }

        var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
        if (student == null)
        {
            TempData["ErrorMessage"] = "Data Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }
        return View(student);
    }

    // POST: Student/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
        }
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Data Siswa berhasil dihapus";
        return RedirectToAction(nameof(Index));
    }

    // GET: Student/Details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "ID Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }

        var student = await _context.Students.FirstOrDefaultAsync(m => m.Id == id);
        if (student == null)
        {
            TempData["ErrorMessage"] = "Data Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }
        return View(student);
    }

    private bool StudentExists(int id)
    {
        return _context.Students.Any(e => e.Id == id);
    }
}
