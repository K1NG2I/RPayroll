using RPayroll.Domain.DTOs.Leave;

namespace RPayroll.Domain.Interfaces.Services;

public interface ILeaveService
{
    Task<LeaveRequestDto> ApplyLeaveAsync(LeaveRequestDto dto);
    Task<LeaveRequestDto?> ApproveLeaveAsync(int leaveId);
    Task<IEnumerable<LeaveRequestDto>> GetLeavesByEmployeeAsync(int employeeId);
}
