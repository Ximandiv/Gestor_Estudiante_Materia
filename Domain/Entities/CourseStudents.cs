namespace Domain.Entities;

/// <summary>
/// Represents the association between a student and a course.
/// </summary>
public class CourseStudents
{
    /// <summary>
    /// Gets or sets the unique identifier for the course-student association.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the unique identifier for the student associated with the course.
    /// </summary>
    public Guid StudentId { get; set; } = Guid.Empty;

    /// <summary>
    /// Gets or sets the unique identifier for the course associated with the student.
    /// </summary>
    public int CourseId { get; set; } = 0;
}
