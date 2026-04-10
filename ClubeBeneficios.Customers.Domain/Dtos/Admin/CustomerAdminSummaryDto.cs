namespace ClubeBeneficios.Customers.Domain.Dtos.Admin;

public sealed class CustomerAdminSummaryDto
{
    public int TotalClients { get; set; }
    public int LeadCount { get; set; }
    public int PendingProfileCount { get; set; }
    public int PendingDocumentsCount { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public int BlockedCount { get; set; }
    public DateTime? LatestCreatedAt { get; set; }
    public DateTime? LatestUpdatedAt { get; set; }
}