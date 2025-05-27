using Xunit;
using Moq;
using Gestor_Estudiante_Materia.Controllers;
using Domain.ViewModels;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Testing;
public class StudentControllerTests : TestBase
{
    [Fact]
    public async Task Can_Create_Student()
    {
        var mockService = new Mock<Application.Interfaces.Services.IStudentService>();
        var logger = Mock.Of<Microsoft.Extensions.Logging.ILogger<StudentController>>();

        mockService.Setup(s => s.CreateStudentAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

        var controller = new StudentController(mockService.Object, logger);
        SetupTempData(controller);
        var vm = new StudentVM { Name = "Juan", Document = "123", Email = "juan@email.com" };

        var result = await controller.Create(vm);

        Assert.IsType<RedirectToActionResult>(result);
        mockService.Verify(s => s.CreateStudentAsync(It.Is<Student>(s =>
            s.Name == vm.Name && s.Document == vm.Document && s.Email == vm.Email)), Times.Once);
    }

    [Fact]
    public async Task Can_Edit_Student()
    {
        var mockService = new Mock<Application.Interfaces.Services.IStudentService>();
        var logger = Mock.Of<Microsoft.Extensions.Logging.ILogger<StudentController>>();

        var studentId = System.Guid.NewGuid();
        var student = new Student { Id = studentId, Name = "Juan", Document = "123", Email = "juan@email.com" };
        mockService.Setup(s => s.GetStudentByIdAsync(studentId)).ReturnsAsync(student);
        mockService.Setup(s => s.UpdateStudentAsync(It.IsAny<Student>())).Returns(Task.CompletedTask);

        var controller = new StudentController(mockService.Object, logger);
        SetupTempData(controller);
        var vm = new StudentVM { Id = studentId.ToString(), Name = "Pedro", Document = "456", Email = "pedro@email.com" };

        var result = await controller.Edit(vm);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Pedro", student.Name); // The controller should update the entity
        mockService.Verify(s => s.UpdateStudentAsync(student), Times.Once);
    }

    [Fact]
    public async Task Can_Delete_Student()
    {
        var mockService = new Mock<Application.Interfaces.Services.IStudentService>();
        var logger = Mock.Of<Microsoft.Extensions.Logging.ILogger<StudentController>>();

        var studentId = System.Guid.NewGuid();
        var student = new Student { Id = studentId, Name = "Juan", Document = "123", Email = "juan@email.com" };
        mockService.Setup(s => s.GetStudentByIdAsync(studentId)).ReturnsAsync(student);
        mockService.Setup(s => s.DeleteStudentAsync(studentId)).Returns(Task.CompletedTask);

        var controller = new StudentController(mockService.Object, logger);
        SetupTempData(controller);
        var result = await controller.Delete(studentId.ToString());

        Assert.IsType<RedirectToActionResult>(result);
        mockService.Verify(s => s.DeleteStudentAsync(studentId), Times.Once);
    }

    [Fact]
    public void StudentVM_Email_Validation()
    {
        var vm = new StudentVM { Name = "Juan", Document = "123", Email = "noemail" };
        var ctx = new ValidationContext(vm);
        var results = new List<ValidationResult>();
        var valid = Validator.TryValidateObject(vm, ctx, results, true);

        Assert.False(valid);
        Assert.Contains(results, r => r.ErrorMessage.Contains("Correo electrónico inválido"));
    }
}
