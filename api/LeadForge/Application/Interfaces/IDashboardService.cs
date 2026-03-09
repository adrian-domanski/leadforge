using LeadForge.Application.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace LeadForge.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponse> GetDashboardAsync(CancellationToken ct);
}