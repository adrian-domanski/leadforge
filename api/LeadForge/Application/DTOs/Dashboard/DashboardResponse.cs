namespace LeadForge.Application.Dashboard;

public class DashboardResponse
{
    public int Credits { get; set; }
    public int GenerationsCount { get; set; }
    public List<GenerationListItem> RecentGenerations { get; set; } = [];
}