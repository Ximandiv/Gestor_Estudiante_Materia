using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services;
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;

    public StudentService(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }

    public async Task<StudentListVM> GetStudentListAsync(int page, int pageSize)
    {
        var students = await _studentRepository.GetStudentsAsync(page, pageSize);
        var total = await _studentRepository.GetTotalStudentsAsync();
        var studentVMs = students.ConvertAll(s => new StudentVM(s));
        return new StudentListVM(studentVMs, total, pageSize, page);
    }

    public Task<Student?> GetStudentByIdAsync(Guid id)
        => _studentRepository.GetByIdAsync(id);

    public Task CreateStudentAsync(Student student)
        => _studentRepository.AddAsync(student);

    public Task UpdateStudentAsync(Student student)
        => _studentRepository.UpdateAsync(student);

    public Task DeleteStudentAsync(Guid id)
        => _studentRepository.DeleteAsync(id);

    public Task<List<Course>> GetAssignedCoursesAsync(Guid studentId)
        => _studentRepository.GetAssignedCoursesAsync(studentId);
}