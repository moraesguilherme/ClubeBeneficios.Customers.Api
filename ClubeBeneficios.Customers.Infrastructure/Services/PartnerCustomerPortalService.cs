using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;
using ClubeBeneficios.Customers.Domain.Dtos.PartnerCustomerPortal;
using ClubeBeneficios.Customers.Domain.Interfaces;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Domain.Services;

namespace ClubeBeneficios.Customers.Infrastructure.Services;

public sealed class PartnerCustomerPortalService : IPartnerCustomerPortalService
{
    private readonly IPartnerCustomerPortalRepository _repository;
    private readonly IUserContext _userContext;

    public PartnerCustomerPortalService(IPartnerCustomerPortalRepository repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<PartnerCustomerBenefitDashboardDto> GetMyBenefitDashboardAsync(CancellationToken cancellationToken = default)
    {
        var partnerCustomerId = await ResolvePartnerCustomerIdAsync(cancellationToken);
        var result = await _repository.GetBenefitDashboardAsync(partnerCustomerId, cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException("Cliente parceiro nÃ£o encontrado para o usuÃ¡rio autenticado.");
        }

        return result;
    }

    public async Task<IReadOnlyCollection<BenefitRequestListItemDto>> GetMyBenefitRequestsAsync(CancellationToken cancellationToken = default)
    {
        var partnerCustomerId = await ResolvePartnerCustomerIdAsync(cancellationToken);
        return await _repository.GetBenefitRequestsAsync(partnerCustomerId, cancellationToken);
    }

    private async Task<Guid> ResolvePartnerCustomerIdAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            throw new UnauthorizedAccessException("UsuÃ¡rio autenticado invÃ¡lido ou nÃ£o informado.");
        }

        var partnerCustomerId = await _repository.GetPartnerCustomerIdByUserIdAsync(userId, cancellationToken);

        if (!partnerCustomerId.HasValue)
        {
            throw new KeyNotFoundException("NÃ£o foi encontrado cliente parceiro vinculado ao usuÃ¡rio autenticado.");
        }

        return partnerCustomerId.Value;
    }
}