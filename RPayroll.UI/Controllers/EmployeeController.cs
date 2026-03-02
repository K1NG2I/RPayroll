using Microsoft.AspNetCore.Mvc;
using RPayroll.Domain.DTOs.Employee;
using RPayroll.UI.Services;

namespace RPayroll.UI.Controllers;

public class EmployeeController : Controller
{
    private readonly ApiClient _apiClient;

    public EmployeeController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult List()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        ViewData["EmployeeId"] = id;
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _apiClient.GetAsync<List<EmployeeDto>>("/api/employee");
        return Ok(employees ?? new List<EmployeeDto>());
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _apiClient.GetAsync<EmployeeDto>($"/api/employee/{id}");
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var employee = await _apiClient.PostAsync<CreateEmployeeDto, EmployeeDto>("/api/employee", dto);
        if (employee == null)
        {
            return BadRequest();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> AddContact(int id, [FromBody] EmployeeContactPersonDto dto)
    {
        var contact = await _apiClient.PostAsync<EmployeeContactPersonDto, EmployeeContactPersonDto>(
            $"/api/employee/{id}/contacts", dto);
        if (contact == null)
        {
            return BadRequest();
        }
        return Ok(contact);
    }
}
