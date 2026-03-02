using RPayroll.Domain.Entities;
using RPayroll.Infrastructure.Repositories.Interfaces;

namespace RPayroll.Infrastructure.Repositories.Implementations;

public class EmployeeRepository : IEmployeeRepository
{
    public Task<Employee?> GetByIdAsync(int id)
    {
        var employee = InMemoryDataStore.Employees.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(employee);
    }

    public Task<List<Employee>> GetAllAsync()
    {
        return Task.FromResult(InMemoryDataStore.Employees.ToList());
    }

    public Task AddAsync(Employee employee)
    {
        employee.Id = InMemoryDataStore.NextEmployeeId();
        AssignContactIds(employee);
        InMemoryDataStore.Employees.Add(employee);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Employee employee)
    {
        var index = InMemoryDataStore.Employees.FindIndex(e => e.Id == employee.Id);
        if (index >= 0)
        {
            AssignContactIds(employee);
            InMemoryDataStore.Employees[index] = employee;
        }
        return Task.CompletedTask;
    }

    private static void AssignContactIds(Employee employee)
    {
        foreach (var contact in employee.ContactPersons)
        {
            if (contact.Id == 0)
            {
                contact.Id = InMemoryDataStore.NextContactId();
            }
        }
    }
}
