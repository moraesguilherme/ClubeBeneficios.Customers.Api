using ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;
using ClubeBeneficios.Customers.Domain.Interfaces;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Domain.Services;

namespace ClubeBeneficios.Customers.Infrastructure.Services;

public sealed class CustomerPortalService : ICustomerPortalService
{
    private readonly ICustomerPortalRepository _repository;
    private readonly IUserContext _userContext;

    public CustomerPortalService(ICustomerPortalRepository repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public async Task<CustomerBenefitDashboardDto> GetMyBenefitDashboardAsync(CancellationToken cancellationToken = default)
    {
        var clientId = await ResolveClientIdAsync(cancellationToken);
        var result = await _repository.GetBenefitDashboardAsync(clientId, cancellationToken);

        if (result is null)
        {
            throw new InvalidOperationException("Cliente nÃ£o encontrado para o usuÃ¡rio autenticado.");
        }

        return result;
    }

    public async Task<IReadOnlyCollection<BenefitRequestListItemDto>> GetMyBenefitRequestsAsync(CancellationToken cancellationToken = default)
    {
        var clientId = await ResolveClientIdAsync(cancellationToken);
        return await _repository.GetBenefitRequestsAsync(clientId, cancellationToken);
    }

    private async Task<Guid> ResolveClientIdAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            throw new UnauthorizedAccessException("UsuÃ¡rio autenticado invÃ¡lido ou nÃ£o informado.");
        }

        var clientId = await _repository.GetClientIdByUserIdAsync(userId, cancellationToken);

        if (!clientId.HasValue)
        {
            throw new KeyNotFoundException("NÃ£o foi encontrado cliente vinculado ao usuÃ¡rio autenticado.");
        }

        return clientId.Value;
    }
}