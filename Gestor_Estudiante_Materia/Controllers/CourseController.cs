using Application.Interfaces.Services;
using Domain.Entities;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_Estudiante_Materia.Controllers;

public class CourseController : Controller
{
    private readonly ILogger<CourseController> _logger;
    private readonly ICourseService _courseService;
    private const int PAGESIZE = 10;

    public CourseController(ICourseService courseService, ILogger<CourseController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var result = await _courseService.GetCourseListAsync(page, PAGESIZE);

        if (!string.IsNullOrEmpty(TempData["IndexModalMsg"] as string))
            ViewBag.IndexModalMsg = TempData["IndexModalMsg"];

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> AssignedStudents(string id)
    {
        if (!int.TryParse(id, out var courseId))
            return NotFound();

        var course = await _courseService.GetCourseByIdAsync(courseId);
        if (course == null)
            return NotFound();

        var assignedStudents = await _courseService.GetAssignedStudentsAsync(courseId);

        ViewBag.CourseName = course.Name;
        return View(assignedStudents);
    }

    [HttpGet]
    public IActionResult Create() => View(new CourseVM());

    [HttpPost]
    public async Task<IActionResult> Create(CourseVM request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Por favor, llena todos los campos correctamente.";
            return View(request);
        }

        var course = new Course
        {
            Name = request.Name,
            Code = request.Code,
            Credits = request.Credits
        };

        try
        {
            await _courseService.CreateCourseAsync(course);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMsg = $"Un error ha ocurrido al crear la materia: {ex.Message}";
            return View(request);
        }

        TempData["IndexModalMsg"] = "Materia creada con éxito!";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        if (!int.TryParse(id, out int courseId))
        {
            TempData["IndexModalMsg"] = "Materia no encontrada";
            return RedirectToAction("Index");
        }

        var course = await _courseService.GetCourseByIdAsync(courseId);

        if (course is null)
        {
            TempData["IndexModalMsg"] = "Materia no encontrada";
            return RedirectToAction("Index");
        }

        var result = new CourseVM(course);
        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CourseVM request)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMsg = "Por favor, llena todos los campos correctamente.";
            return View(request);
        }

        if (!int.TryParse(request.Id, out int courseId))
        {
            TempData["IndexModalMsg"] = "Materia no encontrada";
            return RedirectToAction("Index");
        }

        var course = await _courseService.GetCourseByIdAsync(courseId);

        if (course is null)
        {
            TempData["IndexModalMsg"] = "Materia no encontrada";
            return RedirectToAction("Index");
        }

        try
        {
            course.Update(request.Name, request.Code, request.Credits);

            await _courseService.UpdateCourseAsync(course);
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMsg = $"Un error ha ocurrido al editar la Materia: {ex.Message}";
            return View(request);
        }

        TempData["IndexModalMsg"] = "Materia actualizada con éxito!";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(string id)
    {
        if (!int.TryParse(id, out int courseId))
        {
            TempData["IndexModalMsg"] = "Materia no encontrada";
            return RedirectToAction("Index");
        }

        var course = await _courseService.GetCourseByIdAsync(courseId);

        if (course is null)
        {
            TempData["IndexModalMsg"] = "Materia no encontrada";
            return RedirectToAction("Index");
        }

        await _courseService.DeleteCourseAsync(courseId);

        TempData["IndexModalMsg"] = "Materia eliminada con éxito!";
        return RedirectToAction("Index");
    }
}
