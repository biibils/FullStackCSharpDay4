using StudentMVC.Models;

namespace StudentMVC.Services;

public class StudentService : IStudentService
{
    private readonly List<Student> _students;

    public StudentService()
    {
        _students = new List<Student>();
    }

    public List<Student> GetAllStudents(string? searchName)
    {
        var query = _students.AsQueryable();
        if (!string.IsNullOrEmpty(searchName))
        {
            query = query.Where(s => s.Name.Contains(searchName));
        }
        return query.ToList();
    }

    public Student GetStudentById(int id)
    {
        return _students.FirstOrDefault(s => s.Id == id)!;
    }

    public void AddStudent(Student student)
    {
        student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
        _students.Add(student);
    }

    public void UpdateStudent(Student student)
    {
        var existingStudent = _students.FirstOrDefault(s => s.Id == student.Id);
        if (existingStudent != null)
        {
            existingStudent.Name = student.Name;
            existingStudent.Email = student.Email;
            existingStudent.Age = student.Age;
        }
    }

    public void DeleteStudent(int id)
    {
        _students.RemoveAll(s => s.Id == id);
    }
}
