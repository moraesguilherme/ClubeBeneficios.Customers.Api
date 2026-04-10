namespace ClubeBeneficios.Customers.Domain.Dtos.Admin;

public sealed class PartnerCustomersConversionFunnelDto
{
    public Guid PartnerId { get; set; }
    public string PartnerName { get; set; } = string.Empty;
    public int TotalRecords { get; set; }
    public int PreRegisteredCount { get; set; }
    public int DashboardEnabledCount { get; set; }
    public int ProfileCompletedCount { get; set; }
    public int PetCompletedCount { get; set; }
    public int DocumentsPendingCount { get; set; }
    public int UnderReviewCount { get; set; }
    public int EligibleCount { get; set; }
    public int IneligibleCount { get; set; }
    public DateTime? LatestFirstAccessAt { get; set; }
    public DateTime? LatestLastAccessAt { get; set; }
}