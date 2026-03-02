using Microsoft.AspNetCore.Mvc;
using RPayroll.Domain.DTOs.Role;
using RPayroll.UI.Services;

namespace RPayroll.UI.Controllers;

public class RoleController : Controller
{
    private readonly ApiClient _apiClient;

    public RoleController(ApiClient apiClient)
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
        var roles = await _apiClient.GetAsync<List<RoleDto>>("/api/roles");
        return Ok(roles ?? new List<RoleDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
    {
        var role = await _apiClient.PostAsync<CreateRoleDto, RoleDto>("/api/roles", dto);
        if (role == null)
        {
            return BadRequest();
        }
        return Ok(role);
    }

    [HttpPut]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
    {
        var role = await _apiClient.PutAsync<UpdateRoleDto, RoleDto>($"/api/roles/{id}", dto);
        if (role == null)
        {
            return BadRequest();
        }
        return Ok(role);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _apiClient.DeleteAsync($"/api/roles/{id}");
        return result ? Ok() : NotFound();
    }
}
