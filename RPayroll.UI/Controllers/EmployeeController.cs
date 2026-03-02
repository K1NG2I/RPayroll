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
        var employees = await _apiClient.GetAsync<List<EmployeeDto>>("/api/employees");
        return Ok(employees ?? new List<EmployeeDto>());
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _apiClient.GetAsync<EmployeeDto>($"/api/employees/{id}");
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        var employee = await _apiClient.PostAsync<CreateEmployeeDto, EmployeeDto>("/api/employees", dto);
        if (employee == null)
        {
            return BadRequest();
        }
        return Ok(employee);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        var employee = await _apiClient.PutAsync<UpdateEmployeeDto, EmployeeDto>($"/api/employees/{id}", dto);
        if (employee == null)
        {
            return BadRequest();
        }
        return Ok(employee);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _apiClient.DeleteAsync($"/api/employees/{id}");
        return result ? Ok() : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddContact(int id, [FromBody] EmployeeContactPersonDto dto)
    {
        var contact = await _apiClient.PostAsync<EmployeeContactPersonDto, EmployeeContactPersonDto>(
            $"/api/employees/{id}/contacts", dto);
        if (contact == null)
        {
            return BadRequest();
        }
        return Ok(contact);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateContact(int id, int contactId, [FromBody] EmployeeContactPersonDto dto)
    {
        var contact = await _apiClient.PutAsync<EmployeeContactPersonDto, EmployeeContactPersonDto>(
            $"/api/employees/{id}/contacts/{contactId}", dto);
        if (contact == null)
        {
            return BadRequest();
        }
        return Ok(contact);
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveContact(int id, int contactId)
    {
        var result = await _apiClient.DeleteAsync($"/api/employees/{id}/contacts/{contactId}");
        return result ? Ok() : NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Search(string query)
    {
        var results = await _apiClient.GetAsync<List<EmployeeSearchResultDto>>($"/api/employees/search?query={Uri.EscapeDataString(query ?? string.Empty)}");
        return Ok(results ?? new List<EmployeeSearchResultDto>());
    }
}
