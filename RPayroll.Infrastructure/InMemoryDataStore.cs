using RPayroll.Domain.Entities;
using RPayroll.Domain.Enums;

namespace RPayroll.Infrastructure;

public static class InMemoryDataStore
{
    public static List<Employee> Employees { get; } = new();
    public static List<User> Users { get; } = new();
    public static List<LeaveRequest> LeaveRequests { get; } = new();
    public static List<Payroll> Payrolls { get; } = new();
    public static List<Role> Roles { get; } = new();

    private static int _employeeId = 1;
    private static int _userId = 1;
    private static int _leaveId = 1;
    private static int _payrollId = 1;
    private static int _roleId = 1;
    private static int _contactId = 1;

    static InMemoryDataStore()
    {
        var adminRole = new Role
        {
            Id = NextRoleId(),
            Name = "Admin",
            CreatedDate = DateTime.UtcNow,
            Status = StatusCode.Accepted
        };
        Roles.Add(adminRole);

        var adminUser = new User
        {
            Id = NextUserId(),
            Username = "admin",
            Password = "admin123",
            RoleId = adminRole.Id,
            Role = adminRole,
            CreatedDate = DateTime.UtcNow,
            Status = StatusCode.Accepted
        };
        Users.Add(adminUser);
        adminRole.Users.Add(adminUser);
    }

    public static int NextEmployeeId() => _employeeId++;
    public static int NextUserId() => _userId++;
    public static int NextLeaveId() => _leaveId++;
    public static int NextPayrollId() => _payrollId++;
    public static int NextRoleId() => _roleId++;
    public static int NextContactId() => _contactId++;
}
