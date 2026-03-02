using RPayroll.Domain.Entities;

namespace RPayroll.Infrastructure.Repositories.Interfaces;

public interface IPayrollRepository
{
    Task<Payroll?> GetByIdAsync(int id);
    Task<List<Payroll>> GetAllAsync();
    Task<List<Payroll>> GetByEmployeeAsync(int employeeId);
    Task AddAsync(Payroll payroll);
    Task UpdateAsync(Payroll payroll);
}
