using System.Collections.Generic;

namespace StudentMVC.Models;

public class CourseParticipant
{
	public Course? Course { get; set; }
	public List<Student> AllStudents { get; set; } = new List<Student>();
	public List<int> SelectedStudentIds { get; set; } = new List<int>();
}