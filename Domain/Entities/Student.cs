using Domain.ViewModels;

namespace Domain.Entities;

/// <summary>
/// Represents a student with identification, name, document, and email information.
/// </summary>
public class Student
{
    /// <summary>
    /// Gets or sets the unique identifier for the student.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the full name of the student.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the document number (e.g., student ID or national ID) of the student.
    /// </summary>
    public string Document { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the student.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Updates the student's information with new values for name, document, and email.
    /// </summary>
    /// <param name="newName">New name for student</param>
    /// <param name="newDocument">New document for student</param>
    /// <param name="newEmail">New email for student</param>
    public void Update(
        string newName,
        string newDocument,
        string newEmail)
    {
        Name = newName;
        Document = newDocument;
        Email = newEmail;
    }

    public Student() { }

    public Student(
        StudentVM viewModel)
    {
        Name = viewModel.Name;
        Document = viewModel.Document;
        Email = viewModel.Email;
    }
}