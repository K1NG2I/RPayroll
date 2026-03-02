using RPayroll.Domain.DTOs.Employee;

namespace RPayroll.Domain.Interfaces.Services;

public interface IEmployeeService
{
    Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto);
    Task<EmployeeDto?> GetEmployeeByIdAsync(int id);
    Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
    Task<EmployeeContactPersonDto> AddContactPersonAsync(int employeeId, EmployeeContactPersonDto dto);
}
