using RPayroll.Domain.Entities;
using RPayroll.Infrastructure.Repositories.Interfaces;

namespace RPayroll.Infrastructure.Repositories.Implementations;

public class LeaveRepository : ILeaveRepository
{
    public Task<LeaveRequest?> GetByIdAsync(int id)
    {
        var leave = InMemoryDataStore.LeaveRequests.FirstOrDefault(l => l.Id == id);
        return Task.FromResult(leave);
    }

    public Task<List<LeaveRequest>> GetAllAsync()
    {
        return Task.FromResult(InMemoryDataStore.LeaveRequests.ToList());
    }

    public Task<List<LeaveRequest>> GetByEmployeeAsync(int employeeId)
    {
        var leaves = InMemoryDataStore.LeaveRequests.Where(l => l.EmployeeId == employeeId).ToList();
        return Task.FromResult(leaves);
    }

    public Task AddAsync(LeaveRequest leaveRequest)
    {
        leaveRequest.Id = InMemoryDataStore.NextLeaveId();
        InMemoryDataStore.LeaveRequests.Add(leaveRequest);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(LeaveRequest leaveRequest)
    {
        var index = InMemoryDataStore.LeaveRequests.FindIndex(l => l.Id == leaveRequest.Id);
        if (index >= 0)
        {
            InMemoryDataStore.LeaveRequests[index] = leaveRequest;
        }
        return Task.CompletedTask;
    }
}
