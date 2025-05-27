using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.POCO;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services;
public class AssignCourseService : IAssignCourseService
{
    private readonly IAssignCourseRepository _assignCourseRepository;

    public AssignCourseService(IAssignCourseRepository assignCourseRepository)
    {
        _assignCourseRepository = assignCourseRepository;
    }

    public Task<List<StudentOption>> GetStudentOptionsAsync()
        => _assignCourseRepository.GetStudentOptionsAsync();

    public Task<List<CourseOption>> GetCourseOptionsAsync()
        => _assignCourseRepository.GetCourseOptionsAsync();

    public Task<bool> IsCourseAlreadyAssignedAsync(Guid studentId, int courseId)
        => _assignCourseRepository.IsCourseAlreadyAssignedAsync(studentId, courseId);

    public Task<Course?> GetCourseAsync(int courseId)
        => _assignCourseRepository.GetCourseAsync(courseId);

    public Task<int> GetAssignedHighCreditCoursesCountAsync(Guid studentId)
        => _assignCourseRepository.GetAssignedHighCreditCoursesCountAsync(studentId);

    public Task AssignCourseAsync(Guid studentId, int courseId)
        => _assignCourseRepository.AssignCourseAsync(studentId, courseId);

    public Task<bool> DeleteAssignmentAsync(Guid studentId, int courseId)
        => _assignCourseRepository.DeleteAssignmentAsync(studentId, courseId);
}
