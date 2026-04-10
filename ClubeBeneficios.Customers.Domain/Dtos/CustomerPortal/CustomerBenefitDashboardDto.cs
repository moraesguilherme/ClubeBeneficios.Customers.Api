namespace ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;

public sealed class CustomerBenefitDashboardDto
{
    public CustomerBenefitDashboardSummaryDto Summary { get; set; } = new();
    public IReadOnlyCollection<BenefitRequestListItemDto> RecentRequests { get; set; } = Array.Empty<BenefitRequestListItemDto>();
}