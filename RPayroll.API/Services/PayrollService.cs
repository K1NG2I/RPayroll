using RPayroll.Domain.DTOs.Payroll;
using RPayroll.Domain.Entities;
using RPayroll.Domain.Enums;
using RPayroll.Domain.Interfaces.Services;
using RPayroll.Infrastructure.UnitOfWork;

namespace RPayroll.API.Services;

public class PayrollService : IPayrollService
{
    private readonly IUnitOfWork _unitOfWork;

    public PayrollService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PayrollDto> GeneratePayrollAsync(int employeeId, DateTime periodStart, DateTime periodEnd)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        var gross = employee.BasicSalary + (employee.BasicSalary * 0.2m);
        var net = gross - (employee.BasicSalary * 0.1m);

        var payroll = new Payroll
        {
            EmployeeId = employeeId,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
            GrossSalary = gross,
            NetSalary = net,
            GeneratedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            Status = StatusCode.Accepted
        };

        await _unitOfWork.Payrolls.AddAsync(payroll);
        await _unitOfWork.SaveChangesAsync();

        return MapPayroll(payroll);
    }

    public async Task<IEnumerable<PayrollDto>> GetPayrollsByEmployeeAsync(int employeeId)
    {
        var payrolls = await _unitOfWork.Payrolls.GetByEmployeeAsync(employeeId);
        return payrolls.Select(MapPayroll).ToList();
    }

    private static PayrollDto MapPayroll(Payroll payroll)
    {
        return new PayrollDto
        {
            Id = payroll.Id,
            EmployeeId = payroll.EmployeeId,
            PeriodStart = payroll.PeriodStart,
            PeriodEnd = payroll.PeriodEnd,
            GrossSalary = payroll.GrossSalary,
            NetSalary = payroll.NetSalary,
            GeneratedDate = payroll.GeneratedDate
        };
    }
}
