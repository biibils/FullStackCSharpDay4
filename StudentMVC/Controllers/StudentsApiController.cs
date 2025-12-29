using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentMVC.Data;
using StudentMVC.Models;

namespace StudentMVC.Controllers.Api;

[ApiVersion("1.0")]
[Route("api/students")]
[ApiController]
public class StudentsApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsApiController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/v1/StudentsApi
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudentsV1()
    {
        return await _context.Students.ToListAsync();
    }

    // GET: api/v1/StudentsApi/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudentV1(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }
        return student;
    }

    // PUT: api/v1/StudentsApi/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStudentV1(int id, Student student)
    {
        if (id != student.Id)
        {
            return BadRequest();
        }

        _context.Entry(student).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    // POST: api/v1/StudentsApi
    [HttpPost]
    public async Task<ActionResult<Student>> PostStudentV1(Student student)
    {
        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetStudentV1", new { id = student.Id }, student);
    }

    // DELETE: api/v1/StudentsApi/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudentV1(int id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PATCH: api/v1/StudentsApi/{id}/email
    [HttpPatch("{id}/email")]
    public async Task<ActionResult> UpdateStudentEmail(int id, UpdateStudentEmailDto dto)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null)
            return NotFound();

        student.Email = dto.Email;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool StudentExists(int id)
    {
        return _context.Students.Any(e => e.Id == id);
    }
}

[ApiVersion("2.0")]
[Route("api/students")]
[ApiController]
public class StudentsApiV2Controller : ControllerBase
{
    private readonly AppDbContext _context;

    public StudentsApiV2Controller(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudentV2()
    {
        return await _context
            .Students.Select(s => new Student
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
            })
            .ToListAsync();
    }
}
