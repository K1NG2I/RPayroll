using RPayroll.Domain.Entities;
using RPayroll.Infrastructure.Repositories.Interfaces;

namespace RPayroll.Infrastructure.Repositories.Implementations;

public class PayrollRepository : IPayrollRepository
{
    public Task<Payroll?> GetByIdAsync(int id)
    {
        var payroll = InMemoryDataStore.Payrolls.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(payroll);
    }

    public Task<List<Payroll>> GetAllAsync()
    {
        return Task.FromResult(InMemoryDataStore.Payrolls.ToList());
    }

    public Task<List<Payroll>> GetByEmployeeAsync(int employeeId)
    {
        var payrolls = InMemoryDataStore.Payrolls.Where(p => p.EmployeeId == employeeId).ToList();
        return Task.FromResult(payrolls);
    }

    public Task AddAsync(Payroll payroll)
    {
        payroll.Id = InMemoryDataStore.NextPayrollId();
        InMemoryDataStore.Payrolls.Add(payroll);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Payroll payroll)
    {
        var index = InMemoryDataStore.Payrolls.FindIndex(p => p.Id == payroll.Id);
        if (index >= 0)
        {
            InMemoryDataStore.Payrolls[index] = payroll;
        }
        return Task.CompletedTask;
    }
}
