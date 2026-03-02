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

    [HttpGet]
    public IActionResult List()
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var payrolls = await _apiClient.GetAsync<List<PayrollDto>>("/api/payroll");
        return Ok(payrolls ?? new List<PayrollDto>());
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _apiClient.DeleteAsync($"/api/payroll/{id}");
        return result ? Ok() : NotFound();
    }

    public class PayrollGenerateRequest
    {
        public int EmployeeId { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
