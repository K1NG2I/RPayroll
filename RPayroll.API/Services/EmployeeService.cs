using RPayroll.Domain.DTOs.Employee;
using RPayroll.Domain.Entities;
using RPayroll.Domain.Enums;
using RPayroll.Domain.Interfaces.Services;
using RPayroll.Infrastructure.UnitOfWork;

namespace RPayroll.API.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                DateOfBirth = dto.DateOfBirth,
                Department = dto.Department,
                Position = dto.Position,
                BasicSalary = dto.BasicSalary,
                CreatedDate = DateTime.UtcNow,
                Status = StatusCode.Accepted
            };

            if (dto.ContactPersons.Count > 0)
            {
                foreach (var contact in dto.ContactPersons)
                {
                    employee.ContactPersons.Add(new EmployeeContactPerson
                    {
                        Name = contact.Name,
                        Relationship = contact.Relationship,
                        Phone = contact.Phone,
                        Email = contact.Email,
                        CreatedDate = DateTime.UtcNow,
                        Status = StatusCode.Accepted
                    });
                }
            }

            await _unitOfWork.Employees.AddAsync(employee);

            if (dto.CreateLogin && !string.IsNullOrWhiteSpace(dto.Username))
            {
                var user = new User
                {
                    Username = dto.Username,
                    Password = dto.Password ?? string.Empty,
                    Role = new Role
                    {
                        Name = string.IsNullOrWhiteSpace(dto.RoleName) ? "Employee" : dto.RoleName,
                        CreatedDate = DateTime.UtcNow,
                        Status = StatusCode.Accepted
                    },
                    EmployeeId = employee.Id,
                    Employee = employee,
                    CreatedDate = DateTime.UtcNow,
                    Status = StatusCode.Accepted
                };

                await _unitOfWork.Users.AddAsync(user);

                employee.UserId = user.Id;
                employee.User = user;
                await _unitOfWork.Employees.UpdateAsync(employee);
            }

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            return MapEmployee(employee);
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        return employee == null ? null : MapEmployee(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
    {
        var employees = await _unitOfWork.Employees.GetAllAsync();
        return employees.Select(MapEmployee).ToList();
    }

    public async Task<EmployeeContactPersonDto> AddContactPersonAsync(int employeeId, EmployeeContactPersonDto dto)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId);
        if (employee == null)
        {
            throw new InvalidOperationException("Employee not found.");
        }

        var contact = new EmployeeContactPerson
        {
            EmployeeId = employeeId,
            Name = dto.Name,
            Relationship = dto.Relationship,
            Phone = dto.Phone,
            Email = dto.Email,
            CreatedDate = DateTime.UtcNow,
            Status = StatusCode.Accepted
        };

        employee.ContactPersons.Add(contact);
        await _unitOfWork.Employees.UpdateAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        var mapped = new EmployeeContactPersonDto
        {
            Id = contact.Id,
            EmployeeId = employeeId,
            Name = contact.Name,
            Relationship = contact.Relationship,
            Phone = contact.Phone,
            Email = contact.Email
        };

        return mapped;
    }

    private static EmployeeDto MapEmployee(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            DateOfBirth = employee.DateOfBirth,
            Department = employee.Department,
            Position = employee.Position,
            BasicSalary = employee.BasicSalary,
            ContactPersons = employee.ContactPersons.Select(c => new EmployeeContactPersonDto
            {
                Id = c.Id,
                EmployeeId = employee.Id,
                Name = c.Name,
                Relationship = c.Relationship,
                Phone = c.Phone,
                Email = c.Email
            }).ToList()
        };
    }
}
