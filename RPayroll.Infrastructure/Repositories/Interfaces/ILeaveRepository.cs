using RPayroll.Domain.Entities;

namespace RPayroll.Infrastructure.Repositories.Interfaces;

public interface ILeaveRepository
{
    Task<LeaveRequest?> GetByIdAsync(int id);
    Task<List<LeaveRequest>> GetAllAsync();
    Task<List<LeaveRequest>> GetByEmployeeAsync(int employeeId);
    Task AddAsync(LeaveRequest leaveRequest);
    Task UpdateAsync(LeaveRequest leaveRequest);
}
