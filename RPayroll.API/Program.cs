using RPayroll.Domain.Interfaces.Services;
using RPayroll.Infrastructure.Repositories.Implementations;
using RPayroll.Infrastructure.Repositories.Interfaces;
using RPayroll.Infrastructure.Security;
using RPayroll.Infrastructure.UnitOfWork;
using RPayroll.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<FakeTokenGenerator>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
