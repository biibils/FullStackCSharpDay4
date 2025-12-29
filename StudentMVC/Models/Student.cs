using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace StudentMVC.Models;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Nama harus diisi.")]
    [StringLength(100, ErrorMessage = "Nama tidak boleh lebih dari 100 karakter.")]
    [OnlyLetters(ErrorMessage = "Nama hanya boleh mengandung huruf dan spasi.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Email harus diisi")]
    [EmailAddress(ErrorMessage = "Format email tidak valid.")]
    public required string Email { get; set; }

    [Range(18, 60, ErrorMessage = "Usia antara 18 hingga 60 tahun")]
    public int Age { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}

public class OnlyLettersAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
            return ValidationResult.Success;

        var name = value.ToString();

        if (!Regex.IsMatch(name!, @"^[a-zA-Z\s]+$"))
        {
            return new ValidationResult("Nama hanya boleh mengandung huruf dan spasi.");
        }

        return ValidationResult.Success;
    }
}

// model untuk PATCH Email
public class UpdateStudentEmailDto
{
    public required string Email { get; set; }
}
