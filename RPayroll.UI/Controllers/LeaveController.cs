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

    [HttpGet]
    public IActionResult Approval()
    {
        return View();
    }

    [HttpGet]
    public IActionResult MyLeaves()
    {
        return View();
    }

    [HttpGet]
    public IActionResult History()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Apply([FromBody] LeaveRequestDto dto)
    {
        var result = await _apiClient.PostAsync<LeaveRequestDto, LeaveRequestDto>("/api/leave", dto);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _apiClient.PutAsync<object, LeaveRequestDto>($"/api/leave/{id}/approve", new { });
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Reject(int id)
    {
        var result = await _apiClient.PutAsync<object, LeaveRequestDto>($"/api/leave/{id}/reject", new { });
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var leaves = await _apiClient.GetAsync<List<LeaveRequestDto>>("/api/leave");
        return Ok(leaves ?? new List<LeaveRequestDto>());
    }

    [HttpGet]
    public async Task<IActionResult> GetMy()
    {
        var leaves = await _apiClient.GetAsync<List<LeaveRequestDto>>("/api/leave/my");
        return Ok(leaves ?? new List<LeaveRequestDto>());
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _apiClient.DeleteAsync($"/api/leave/{id}");
        return result ? Ok() : NotFound();
    }
}
