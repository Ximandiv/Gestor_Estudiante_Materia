using Domain.Entities;

namespace Application.Interfaces.Repositories;
public interface ICourseRepository
{
    Task<List<Course>> GetCoursesAsync(int page, int pageSize);
    Task<int> GetTotalCoursesAsync();
    Task<Course?> GetByIdAsync(int id);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(int id);
    Task<List<Student>> GetAssignedStudentsAsync(int courseId);
}
