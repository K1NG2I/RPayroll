using Microsoft.AspNetCore.Mvc;
using RPayroll.Domain.DTOs.Leave;
using RPayroll.Domain.Interfaces.Services;

namespace RPayroll.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private readonly ILeaveService _leaveService;

    public LeaveController(ILeaveService leaveService)
    {
        _leaveService = leaveService;
    }

    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] LeaveRequestDto dto)
    {
        var result = await _leaveService.ApplyLeaveAsync(dto);
        return Ok(result);
    }

    [HttpPost("{id:int}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var result = await _leaveService.ApproveLeaveAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        var leaves = await _leaveService.GetLeavesByEmployeeAsync(employeeId);
        return Ok(leaves);
    }
}
