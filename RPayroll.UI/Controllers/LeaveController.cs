using Microsoft.AspNetCore.Mvc;
using RPayroll.Domain.DTOs.Leave;
using RPayroll.UI.Services;

namespace RPayroll.UI.Controllers;

public class LeaveController : Controller
{
    private readonly ApiClient _apiClient;

    public LeaveController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult Apply()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] LeaveRequestDto dto)
    {
        var result = await _apiClient.PostAsync<LeaveRequestDto, LeaveRequestDto>("/api/leave/apply", dto);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _apiClient.PostAsync<object, LeaveRequestDto>($"/api/leave/{id}/approve", new { });
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        var leaves = await _apiClient.GetAsync<List<LeaveRequestDto>>($"/api/leave/employee/{employeeId}");
        return Ok(leaves ?? new List<LeaveRequestDto>());
    }
}
