using Domain.POCO;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels;

public class AssignCourseVM
{
    [Required]
    [Display(Name = "Estudiante")]
    public Guid StudentId { get; set; }

    [Required]
    [Display(Name = "Materia")]
    public int CourseId { get; set; }

    // For dropdowns
    public List<StudentOption> Students { get; set; } = new();
    public List<CourseOption> Courses { get; set; } = new();
}
