using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentMVC.Data;
using StudentMVC.Models;

// using StudentMVC.Services;

namespace StudentMVC.Controllers;

public class AttendanceController : Controller
{
    private readonly AppDbContext _context;

    public AttendanceController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var attendances = await _context
            .Attendances.Include(a => a.Student)
            .OrderByDescending(a => a.Date)
            .ThenByDescending(a => a.Time)
            .ToListAsync();

        return View(attendances);
    }

    // GET: Attendance/Create/{studentId}
    [HttpGet]
    public async Task<IActionResult> Create(int studentId)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            TempData["ErrorMessage"] = "ID Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }

        var attendance = new Attendance
        {
            StudentId = studentId,
            Student = student,
            Date = DateTime.Now,
            Time = DateTime.Now,
        };
        return View(attendance);
    }

    // POST: Attendance/Create/{studentId}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Attendance attendance)
    {
        var student = _context.Students.Find(attendance.StudentId);
        if (student == null)
        {
            TempData["ErrorMessage"] = "ID Siswa tidak ditemukan";
            return RedirectToAction("Index", "Student");
        }

        attendance.Student = student;

        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Data Presensi tidak valid";
            return View(attendance);
        }
        _context.Add(attendance);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Presensi berhasil disimpan";
        return RedirectToAction(nameof(Index));
    }
}
