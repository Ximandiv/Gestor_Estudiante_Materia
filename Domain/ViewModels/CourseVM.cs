using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels;

public class CourseVM
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Código")]
    public string Code { get; set; } = string.Empty;

    [Display(Name = "Créditos")]
    [Range(1, 100, ErrorMessage = "Los créditos deben ser mayores que 0 y menores que 100.")]
    public int Credits { get; set; } = 0;

    public CourseVM() { }

    public CourseVM(Course course)
    {
        Id = course.Id.ToString();
        Name = course.Name;
        Code = course.Code;
        Credits = course.Credits;
    }
}