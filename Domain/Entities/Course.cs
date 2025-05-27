namespace Domain.Entities;

/// <summary>
/// Represents a course with a name, code, and credits.
/// </summary>
public class Course
{
    /// <summary>
    /// Gets or sets the unique identifier for the course.
    /// </summary>
    public int Id { get; set; } = 0;

    /// <summary>
    /// Gets or sets the name of the course.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique code for the course.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of credits associated with the course.
    /// </summary>
    public int Credits { get; set; } = 0;

    public void Update(
        string newName,
        string newCode,
        int newCredits)
    {
        Name = newName;
        Code = newCode;
        Credits = newCredits;
    }
}
