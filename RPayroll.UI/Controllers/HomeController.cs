using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RPayroll.UI.Models;
using RPayroll.UI.Services;

namespace RPayroll.UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApiClient _apiClient;

    public HomeController(ILogger<HomeController> logger, ApiClient apiClient)
    {
        _logger = logger;
        _apiClient = apiClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAttendanceStats()
    {
        var result = await _apiClient.GetAsync<AttendanceStatsDto>("/api/dashboard/attendance-stats");
        return Ok(result ?? new AttendanceStatsDto());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public class AttendanceStatsDto
    {
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Marked { get; set; }
        public int Late { get; set; }
    }
}
