using LeadForge.Application.Dashboard;
using LeadForge.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeadForge.Api.Controllers;

[ApiController]
[Route("/api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;

    }


    [HttpGet]
        public async Task<ActionResult<DashboardResponse>> GetDashboard(
            CancellationToken ct)
    {
        var result = await _dashboardService.GetDashboardAsync(ct);

        return Ok(result);
    }


}