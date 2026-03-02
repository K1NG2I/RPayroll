namespace RPayroll.Domain.DTOs.Employee;

public class UpdateEmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Department { get; set; }
    public string? Position { get; set; }
    public decimal BasicSalary { get; set; }
    public int? ManagerId { get; set; }
    public DateTime? DateOfJoining { get; set; }
    public string? Address { get; set; }
}
