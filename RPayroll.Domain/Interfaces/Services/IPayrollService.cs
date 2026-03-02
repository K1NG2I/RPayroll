using RPayroll.Domain.DTOs.Payroll;

namespace RPayroll.Domain.Interfaces.Services;

public interface IPayrollService
{
    Task<PayrollDto> GeneratePayrollAsync(int employeeId, DateTime periodStart, DateTime periodEnd);
    Task<IEnumerable<PayrollDto>> GetPayrollsByEmployeeAsync(int employeeId);
}
