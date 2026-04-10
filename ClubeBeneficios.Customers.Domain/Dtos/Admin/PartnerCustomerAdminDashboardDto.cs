namespace ClubeBeneficios.Customers.Domain.Dtos.Admin;

public sealed class PartnerCustomerAdminDashboardDto
{
    public PartnerCustomerAdminSummaryDto Summary { get; set; } = new();
    public IReadOnlyCollection<PartnerCustomersConversionFunnelDto> ConversionFunnels { get; set; } = Array.Empty<PartnerCustomersConversionFunnelDto>();
}