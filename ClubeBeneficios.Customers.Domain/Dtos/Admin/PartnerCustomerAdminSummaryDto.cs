namespace ClubeBeneficios.Customers.Domain.Dtos.Admin;

public sealed class PartnerCustomerAdminSummaryDto
{
    public int TotalPartnerCustomers { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public int BlockedCount { get; set; }
    public int PreRegisteredCount { get; set; }
    public int DashboardEnabledCount { get; set; }
    public int ProfileCompletedCount { get; set; }
    public int PetCompletedCount { get; set; }
    public int DocumentsPendingCount { get; set; }
    public int UnderReviewCount { get; set; }
    public int EligibleCount { get; set; }
    public int IneligibleCount { get; set; }
    public DateTime? LatestCreatedAt { get; set; }
    public DateTime? LatestUpdatedAt { get; set; }
}