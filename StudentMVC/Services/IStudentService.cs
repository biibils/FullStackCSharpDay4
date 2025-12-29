using StudentMVC.Models;

namespace StudentMVC.Services;

public interface IStudentService
{
    List<Student> GetAllStudents(string? searchName);
    Student GetStudentById(int id);
    void AddStudent(Student student);
    void UpdateStudent(Student student);
    void DeleteStudent(int id);
}
