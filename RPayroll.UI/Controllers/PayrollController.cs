using Microsoft.AspNetCore.Mvc;
using RPayroll.Domain.DTOs.Payroll;
using RPayroll.UI.Services;

namespace RPayroll.UI.Controllers;

public class PayrollController : Controller
{
    private readonly ApiClient _apiClient;

    public PayrollController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult Generate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Generate([FromBody] PayrollGenerateRequest dto)
    {
        var result = await _apiClient.PostAsync<PayrollGenerateRequest, PayrollDto>("/api/payroll/generate", dto);
        if (result == null)
        {
            return BadRequest();
        }
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        var payrolls = await _apiClient.GetAsync<List<PayrollDto>>($"/api/payroll/employee/{employeeId}");
        return Ok(payrolls ?? new List<PayrollDto>());
    }

    public class PayrollGenerateRequest
    {
        public int EmployeeId { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
