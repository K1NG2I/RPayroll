using RPayroll.Domain.Common;

namespace RPayroll.Domain.Entities;

public class Employee : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public decimal BasicSalary { get; set; }
    public DateTime? DateOfJoining { get; set; }
    public string? Address { get; set; }

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }
    public List<Employee> Subordinates { get; set; } = new();

    public int? UserId { get; set; }
    public User? User { get; set; }

    public List<EmployeeContactPerson> ContactPersons { get; set; } = new();
    public List<LeaveRequest> LeaveRequests { get; set; } = new();
    public List<Payroll> Payrolls { get; set; } = new();
    public List<Attendance> Attendances { get; set; } = new();
}
