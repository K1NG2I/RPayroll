using Microsoft.AspNetCore.Mvc;
using RPayroll.Domain.DTOs.Attendance;
using RPayroll.Domain.DTOs.Employee;
using RPayroll.UI.Services;

namespace RPayroll.UI.Controllers;

public class AttendanceController : Controller
{
    private readonly ApiClient _apiClient;

    public AttendanceController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult Marking()
    {
        return View();
    }

    [HttpGet]
    public IActionResult List()
    {
        return View();
    }

    [HttpGet]
    public IActionResult My()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Today()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAttendanceDto dto)
    {
        var result = await _apiClient.PostAsync<CreateAttendanceDto, AttendanceDto>("/api/attendance", dto);
        return result == null ? BadRequest() : Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAttendanceDto dto)
    {
        var result = await _apiClient.PutAsync<UpdateAttendanceDto, AttendanceDto>($"/api/attendance/{id}", dto);
        return result == null ? BadRequest() : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        var result = await _apiClient.GetAsync<List<AttendanceDto>>($"/api/attendance/employee/{employeeId}");
        return Ok(result ?? new List<AttendanceDto>());
    }

    [HttpGet]
    public async Task<IActionResult> GetByMonth(int employeeId, int month, int year)
    {
        var result = await _apiClient.GetAsync<List<AttendanceDto>>($"/api/attendance/month/{employeeId}?month={month}&year={year}");
        return Ok(result ?? new List<AttendanceDto>());
    }

    [HttpGet]
    public async Task<IActionResult> GetByDate(string date)
    {
        var result = await _apiClient.GetAsync<List<AttendanceDto>>($"/api/attendance/date/{date}");
        return Ok(result ?? new List<AttendanceDto>());
    }

    [HttpGet]
    public async Task<IActionResult> GetMy()
    {
        var result = await _apiClient.GetAsync<List<AttendanceDto>>("/api/attendance/my");
        return Ok(result ?? new List<AttendanceDto>());
    }

    [HttpGet]
    public async Task<IActionResult> GetMyProfile()
    {
        var profile = await _apiClient.GetAsync<EmployeeDto>("/api/employees/me");
        return profile == null ? NotFound() : Ok(profile);
    }
}
