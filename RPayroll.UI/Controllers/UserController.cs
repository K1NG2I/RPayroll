using Microsoft.AspNetCore.Mvc;
using RPayroll.Domain.DTOs.User;
using RPayroll.UI.Services;

namespace RPayroll.UI.Controllers;

public class UserController : Controller
{
    private readonly ApiClient _apiClient;

    public UserController(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    [HttpGet]
    public IActionResult List()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _apiClient.GetAsync<List<UserDto>>("/api/users");
        return Ok(users ?? new List<UserDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        var user = await _apiClient.PostAsync<CreateUserDto, UserDto>("/api/users", dto);
        if (user == null)
        {
            return BadRequest();
        }
        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
    {
        var user = await _apiClient.PutAsync<UpdateUserDto, UserDto>($"/api/users/{id}", dto);
        if (user == null)
        {
            return BadRequest();
        }
        return Ok(user);
    }

    [HttpDelete]
    public async Task<IActionResult> Disable(int id)
    {
        var result = await _apiClient.DeleteAsync($"/api/users/{id}");
        return result ? Ok() : NotFound();
    }
}
