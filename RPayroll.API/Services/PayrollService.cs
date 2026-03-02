using RPayroll.Domain.DTOs.Payroll;
using RPayroll.Domain.Entities;
using RPayroll.Domain.Enums;
using RPayroll.Domain.Interfaces.Services;
using RPayroll.Infrastructure.UnitOfWork;

namespace RPayroll.API.Services;

public class PayrollService : IPayrollService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserContext _currentUser;

    public PayrollService(IUnitOfWork unitOfWork, ICurrentUserContext currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<PayrollDto> GeneratePayrollAsync(int employeeId, DateTime periodStart, DateTime periodEnd)
    {
        EnsureAuthenticated();
        if (!await CanViewEmployeeAsync(employeeId))
        {
            throw new UnauthorizedAccessException("Not allowed to generate payroll.");
        }

        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId, includeInactive: true);
        if (employee == null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        var monthStart = new DateTime(periodStart.Year, periodStart.Month, 1);
        var totalDaysInMonth = DateTime.DaysInMonth(periodStart.Year, periodStart.Month);
        var monthEnd = monthStart.AddDays(totalDaysInMonth - 1);

        var joiningDate = employee.DateOfJoining?.Date;
        if (joiningDate.HasValue && joiningDate.Value > monthEnd)
        {
            return await CreateZeroPayrollAsync(employeeId, periodStart, periodEnd);
        }

        var workingDays = totalDaysInMonth;
        if (joiningDate.HasValue && joiningDate.Value > monthStart)
        {
            var daysBeforeJoining = (joiningDate.Value - monthStart).Days;
            workingDays -= daysBeforeJoining;
        }

        var approvedLeaves = await _unitOfWork.Leaves.GetByEmployeeAsync(employeeId, includeInactive: false);
        var approvedLeaveDays = approvedLeaves
            .Where(l => l.Status == StatusCode.Accepted)
            .Sum(l => CalculateOverlapDays(l.StartDate.Date, l.EndDate.Date, monthStart, monthEnd));

        workingDays -= approvedLeaveDays;
        if (workingDays < 0)
        {
            workingDays = 0;
        }

        var grossSalary = employee.BasicSalary;
        var dailyRate = totalDaysInMonth == 0 ? 0 : grossSalary / totalDaysInMonth;
        var netSalary = dailyRate * workingDays;

        var payroll = new Payroll
        {
            EmployeeId = employeeId,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
            GrossSalary = grossSalary,
            NetSalary = netSalary,
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
        EnsureAuthenticated();
        if (!await CanViewEmployeeAsync(employeeId))
        {
            throw new UnauthorizedAccessException("Not allowed to view payrolls.");
        }

        var payrolls = await _unitOfWork.Payrolls.GetByEmployeeAsync(employeeId, includeInactive: false);
        return payrolls.Select(MapPayroll).ToList();
    }

    public async Task<IEnumerable<PayrollDto>> GetPayrollsForCurrentUserAsync()
    {
        EnsureAuthenticated();
        var payrolls = await _unitOfWork.Payrolls.GetAllAsync(includeInactive: false);
        var filtered = await ApplyVisibilityFilterAsync(payrolls);
        return filtered.Select(MapPayroll).ToList();
    }

    public async Task<bool> DeletePayrollAsync(int payrollId)
    {
        EnsureAuthenticated();
        if (!IsAdmin())
        {
            throw new UnauthorizedAccessException("Only Admin can delete payroll.");
        }

        var payroll = await _unitOfWork.Payrolls.GetByIdAsync(payrollId, includeInactive: true);
        if (payroll == null)
        {
            return false;
        }

        payroll.Status = StatusCode.Rejected;
        payroll.UpdatedDate = DateTime.UtcNow;
        await _unitOfWork.Payrolls.UpdateAsync(payroll);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    private async Task<IEnumerable<Payroll>> ApplyVisibilityFilterAsync(IEnumerable<Payroll> payrolls)
    {
        if (IsAdmin() || IsHr())
        {
            return payrolls;
        }

        if (IsManager())
        {
            var managerEmployeeId = _currentUser.EmployeeId ?? 0;
            var employees = await _unitOfWork.Employees.GetAllAsync(includeInactive: false);
            var subordinateIds = employees.Where(e => e.ManagerId == managerEmployeeId).Select(e => e.Id).ToHashSet();
            return payrolls.Where(p => subordinateIds.Contains(p.EmployeeId));
        }

        if (_currentUser.EmployeeId.HasValue)
        {
            return payrolls.Where(p => p.EmployeeId == _currentUser.EmployeeId.Value);
        }

        return Enumerable.Empty<Payroll>();
    }

    private async Task<bool> CanViewEmployeeAsync(int employeeId)
    {
        if (IsAdmin() || IsHr())
        {
            return true;
        }

        if (_currentUser.EmployeeId.HasValue && _currentUser.EmployeeId.Value == employeeId)
        {
            return true;
        }

        if (IsManager())
        {
            var managerEmployeeId = _currentUser.EmployeeId ?? 0;
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId, includeInactive: true);
            return employee != null && employee.ManagerId == managerEmployeeId;
        }

        return false;
    }

    private static int CalculateOverlapDays(DateTime start, DateTime end, DateTime rangeStart, DateTime rangeEnd)
    {
        if (end < rangeStart || start > rangeEnd)
        {
            return 0;
        }

        var effectiveStart = start < rangeStart ? rangeStart : start;
        var effectiveEnd = end > rangeEnd ? rangeEnd : end;
        return (effectiveEnd - effectiveStart).Days + 1;
    }

    private async Task<PayrollDto> CreateZeroPayrollAsync(int employeeId, DateTime periodStart, DateTime periodEnd)
    {
        var payroll = new Payroll
        {
            EmployeeId = employeeId,
            PeriodStart = periodStart,
            PeriodEnd = periodEnd,
            GrossSalary = 0,
            NetSalary = 0,
            GeneratedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            Status = StatusCode.Accepted
        };

        await _unitOfWork.Payrolls.AddAsync(payroll);
        await _unitOfWork.SaveChangesAsync();

        return MapPayroll(payroll);
    }

    private bool IsAdmin() => string.Equals(_currentUser.Role, "Admin", StringComparison.OrdinalIgnoreCase);

    private bool IsHr() => string.Equals(_currentUser.Role, "HR", StringComparison.OrdinalIgnoreCase);

    private bool IsManager() => string.Equals(_currentUser.Role, "Manager", StringComparison.OrdinalIgnoreCase);

    private void EnsureAuthenticated()
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }
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
