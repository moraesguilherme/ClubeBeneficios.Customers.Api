using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;
using ClubeBeneficios.Customers.Domain.Dtos.PartnerCustomerPortal;

namespace ClubeBeneficios.Customers.Domain.Services;

public interface IPartnerCustomerPortalService
{
    Task<PartnerCustomerBenefitDashboardDto> GetMyBenefitDashboardAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BenefitRequestListItemDto>> GetMyBenefitRequestsAsync(CancellationToken cancellationToken = default);
}