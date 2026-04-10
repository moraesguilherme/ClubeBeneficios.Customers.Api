using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;
using ClubeBeneficios.Customers.Domain.Dtos.PartnerCustomerPortal;

namespace ClubeBeneficios.Customers.Domain.Repositories;

public interface IPartnerCustomerPortalRepository
{
    Task<Guid?> GetPartnerCustomerIdByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<PartnerCustomerBenefitDashboardDto?> GetBenefitDashboardAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BenefitRequestListItemDto>> GetBenefitRequestsAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default);
}