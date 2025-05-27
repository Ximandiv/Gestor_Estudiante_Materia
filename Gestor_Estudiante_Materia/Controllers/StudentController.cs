using Application.Interfaces.Services;
using Domain.Entities;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_Estudiante_Materia.Controllers;

public class StudentController : Controller
{
    private readonly ILogger<StudentController> _logger;
    private readonly IStudentService _studentService;

    private const int PAGESIZE = 10;

    public StudentController(
        IStudentService studentService,
        ILogger<StudentController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var result = await _studentService.GetStudentListAsync(page, PAGESIZE);

        if (!string.IsNullOrEmpty(TempData["IndexModalMsg"] as string))
            ViewBag.IndexModalMsg = TempData["IndexModalMsg"];

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> AssignedCourses(string id)
    {
        if (!Guid.TryParse(id, out var studentId))
        {
            TempData["IndexModalMsg"] = "Estudiante no encontrado";
            return RedirectToAction("Index");
        }

        var student = await _studentService.GetStudentByIdAsync(studentId);
        if (student is null)
        {
            TempData["IndexModalMsg"] = "Estudiante no encontrado";
            return RedirectToAction("Index");
        }

        var assignedCourses = await _studentService.GetAssignedCoursesAsync(studentId);

        ViewBag.StudentName = student.Name;
        return View(assignedCourses);
    }

    [HttpGet]
    public IActionResult Create() => View(new StudentVM());

    [HttpPost]
    public async Task<IActionResult> Create(StudentVM request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Porfavor, llena todos los campos correctamente.";
            return View(request);
        }

        var student = new Student(request);

        try
        {
            await _studentService.CreateStudentAsync(student);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMsg = $"Un error ha ocurrido al crear el estudiante: {ex.Message}";
            return View(request);
        }

        TempData["IndexModalMsg"] = "Estudiante creado con éxito!";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        if (!Guid.TryParse(id, out var studentId))
        {
            TempData["IndexModalMsg"] = "Estudiante no encontrado";
            return RedirectToAction("Index");
        }

        var student = await _studentService.GetStudentByIdAsync(studentId);

        if (student is null)
        {
            TempData["IndexModalMsg"] = "Estudiante no encontrado";
            return RedirectToAction("Index");
        }

        var result = new StudentVM(student);

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(StudentVM request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Porfavor, llena todos los campos correctamente.";
            return View(request);
        }

        if (!Guid.TryParse(request.Id, out var studentId))
        {
            TempData["IndexModalMsg"] = "Estudiante no encontrado";
            return RedirectToAction("Index");
        }

        var student = await _studentService.GetStudentByIdAsync(studentId);

        if (student is null)
        {
            TempData["IndexModalMsg"] = "Estudiante no encontrado";
            return RedirectToAction("Index");
        }

        try
        {
            student.Update(request.Name, request.Document, request.Email);
            await _studentService.UpdateStudentAsync(student);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMsg = $"Un error ha ocurrido al editar el estudiante: {ex.Message}";
            return View(request);
        }

        TempData["IndexModalMsg"] = "Estudiante actualizado con éxito!";

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(string id)
    {
        if (!Guid.TryParse(id, out var studentId))
        {
            TempData["IndexModalMsg"] = "Estudiante no encontrado";
            return RedirectToAction("Index");
        }

        var student = await _studentService.GetStudentByIdAsync(studentId);

        if (student is null)
        {
            TempData["IndexModalMsg"] = "Estudiante no encontrado";
            return RedirectToAction("Index");
        }

        await _studentService.DeleteStudentAsync(studentId);

        TempData["IndexModalMsg"] = "Estudiante eliminado con éxito!";

        return RedirectToAction("Index");
    }
}
