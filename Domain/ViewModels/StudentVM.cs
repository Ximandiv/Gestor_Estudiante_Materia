using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels;

public class StudentVM
{
    public string Id { get; set; } = string.Empty;

    [Display(Name = "Nombre")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Documento")]
    public string Document { get; set; } = string.Empty;

    [Display(Name = "Correo Electrónico")]
    [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
    public string Email { get; set; } = string.Empty;

    public StudentVM() { }

    public StudentVM(
        Student student)
    {
        Id = student.Id.ToString();
        Name = student.Name;
        Document = student.Document;
        Email = student.Email;
    }
}
