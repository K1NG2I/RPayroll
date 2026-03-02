using Microsoft.AspNetCore.Mvc;
using RPayroll.Domain.DTOs.Auth;
using RPayroll.UI.Services;

namespace RPayroll.UI.Controllers;

public class AuthController : Controller
{
    private readonly ApiClient _apiClient;
    private readonly TokenStore _tokenStore;

    public AuthController(ApiClient apiClient, TokenStore tokenStore)
    {
        _apiClient = apiClient;
        _tokenStore = tokenStore;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _apiClient.PostAsync<LoginRequestDto, LoginResponseDto>("/api/auth/login", request);
        if (response == null)
        {
            return Unauthorized();
        }

        _tokenStore.SetToken(response.Token);
        return Ok(response);
    }
}
