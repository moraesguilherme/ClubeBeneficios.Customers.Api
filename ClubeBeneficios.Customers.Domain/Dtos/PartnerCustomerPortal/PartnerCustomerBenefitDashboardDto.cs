using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;

namespace ClubeBeneficios.Customers.Domain.Dtos.PartnerCustomerPortal;

public sealed class PartnerCustomerBenefitDashboardDto
{
    public PartnerCustomerBenefitDashboardSummaryDto Summary { get; set; } = new();
    public IReadOnlyCollection<BenefitRequestListItemDto> RecentRequests { get; set; } = Array.Empty<BenefitRequestListItemDto>();
}