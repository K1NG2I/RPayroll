using RPayroll.Domain.DTOs.Leave;
using RPayroll.Domain.Entities;
using RPayroll.Domain.Enums;
using RPayroll.Domain.Interfaces.Services;
using RPayroll.Infrastructure.UnitOfWork;

namespace RPayroll.API.Services;

public class LeaveService : ILeaveService
{
    private readonly IUnitOfWork _unitOfWork;

    public LeaveService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LeaveRequestDto> ApplyLeaveAsync(LeaveRequestDto dto)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(dto.EmployeeId);
        if (employee == null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        var existingLeaves = await _unitOfWork.Leaves.GetByEmployeeAsync(dto.EmployeeId);
        var hasOverlap = existingLeaves.Any(l =>
            l.Status != StatusCode.Rejected &&
            dto.StartDate <= l.EndDate &&
            dto.EndDate >= l.StartDate);

        if (hasOverlap)
        {
            throw new InvalidOperationException("Overlapping leave request exists.");
        }

        var leave = new LeaveRequest
        {
            EmployeeId = dto.EmployeeId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Reason = dto.Reason,
            CreatedDate = DateTime.UtcNow,
            Status = StatusCode.Pending
        };

        await _unitOfWork.Leaves.AddAsync(leave);
        await _unitOfWork.SaveChangesAsync();

        return MapLeave(leave);
    }

    public async Task<LeaveRequestDto?> ApproveLeaveAsync(int leaveId)
    {
        var leave = await _unitOfWork.Leaves.GetByIdAsync(leaveId);
        if (leave == null)
        {
            return null;
        }

        leave.Status = StatusCode.Accepted;
        leave.UpdatedDate = DateTime.UtcNow;
        await _unitOfWork.Leaves.UpdateAsync(leave);
        await _unitOfWork.SaveChangesAsync();

        return MapLeave(leave);
    }

    public async Task<IEnumerable<LeaveRequestDto>> GetLeavesByEmployeeAsync(int employeeId)
    {
        var leaves = await _unitOfWork.Leaves.GetByEmployeeAsync(employeeId);
        return leaves.Select(MapLeave).ToList();
    }

    private static LeaveRequestDto MapLeave(LeaveRequest leave)
    {
        return new LeaveRequestDto
        {
            Id = leave.Id,
            EmployeeId = leave.EmployeeId,
            StartDate = leave.StartDate,
            EndDate = leave.EndDate,
            Reason = leave.Reason,
            Status = leave.Status
        };
    }
}
