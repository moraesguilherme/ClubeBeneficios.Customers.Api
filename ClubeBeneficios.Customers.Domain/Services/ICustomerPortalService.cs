using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;

namespace ClubeBeneficios.Customers.Domain.Services;

public interface ICustomerPortalService
{
    Task<CustomerBenefitDashboardDto> GetMyBenefitDashboardAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<BenefitRequestListItemDto>> GetMyBenefitRequestsAsync(CancellationToken cancellationToken = default);
}