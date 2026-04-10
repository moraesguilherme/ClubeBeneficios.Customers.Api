using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;

namespace ClubeBeneficios.Customers.Domain.Repositories;

public interface ICustomerPortalRepository
{
    Task<Guid?> GetClientIdByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CustomerBenefitDashboardDto?> GetBenefitDashboardAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BenefitRequestListItemDto>> GetBenefitRequestsAsync(Guid clientId, CancellationToken cancellationToken = default);
}