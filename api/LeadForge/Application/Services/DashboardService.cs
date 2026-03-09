using LeadForge.Application.Dashboard;
using LeadForge.Application.Interfaces;
using LeadForge.Domain.Exceptions;
using LeadForge.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeadForge.Application;

public class DashboardService : IDashboardService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly AppDbContext _db;

    public DashboardService(ICurrentUserService currentUserService, AppDbContext db)
    {
        _currentUserService = currentUserService;
        _db = db;
    }

    public async Task<DashboardResponse> GetDashboardAsync(CancellationToken ct)
    {
        var userId = _currentUserService.GetUserId();

        var user = await _db.Users
            .Where(u => u.Id == userId)
            .Select(u => new { u.Credits })
            .SingleAsync(ct);

        var generationsCount = await _db.Generations
            .Where(g => g.UserId == userId)
            .CountAsync(ct);

        var recentGenerations = await _db.Generations
            .Where(g => g.UserId == userId)
            .OrderByDescending(g => g.CreatedAt)
            .Take(5)
            .Select(g => new GenerationListItem
            {
                Id = g.Id,
                GoalType = g.GoalType,
                InputText = g.InputText,
                OutputText = g.OutputText,
                CreatedAt = g.CreatedAt
            })
            .ToListAsync(ct);

        return new DashboardResponse
        {
            Credits = user.Credits,
            GenerationsCount = generationsCount,
            RecentGenerations = recentGenerations
        };
    }
}