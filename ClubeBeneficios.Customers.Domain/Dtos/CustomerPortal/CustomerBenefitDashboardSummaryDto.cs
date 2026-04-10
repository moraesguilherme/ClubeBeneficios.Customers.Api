namespace ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;

public sealed class CustomerBenefitDashboardSummaryDto
{
    public Guid ClientId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = string.Empty;
    public int TotalRequests { get; set; }
    public int RequestedCount { get; set; }
    public int PendingReviewCount { get; set; }
    public int UnderReviewCount { get; set; }
    public int ApprovedCount { get; set; }
    public int DeclinedCount { get; set; }
    public int CancelledCount { get; set; }
    public int ExpiredCount { get; set; }
    public int ConvertedToUsageCount { get; set; }
    public int TotalUsages { get; set; }
    public DateTime? LastRequestAt { get; set; }
    public DateTime? LastUsageAt { get; set; }
}