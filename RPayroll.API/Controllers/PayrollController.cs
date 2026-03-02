using Microsoft.AspNetCore.Mvc;
using RPayroll.API.Models;
using RPayroll.Domain.Interfaces.Services;

namespace RPayroll.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PayrollController : ControllerBase
{
    private readonly IPayrollService _payrollService;

    public PayrollController(IPayrollService payrollService)
    {
        _payrollService = payrollService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GeneratePayrollRequest request)
    {
        var payroll = await _payrollService.GeneratePayrollAsync(
            request.EmployeeId,
            request.PeriodStart,
            request.PeriodEnd);
        return Ok(payroll);
    }

    [HttpGet("employee/{employeeId:int}")]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        var payrolls = await _payrollService.GetPayrollsByEmployeeAsync(employeeId);
        return Ok(payrolls);
    }
}
