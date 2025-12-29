using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentMVC.Models;

public class Course
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama Kursus harus diisi")]
    [StringLength(100, ErrorMessage = "Nama tidak boleh lebih dari 100 karakter")]
    public required string Name { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();

    [NotMapped]
    public int Participant => Students?.Count ?? 0;
}
