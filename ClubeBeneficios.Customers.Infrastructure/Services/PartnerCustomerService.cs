using ClubeBeneficios.Customers.Domain.Dtos;
using ClubeBeneficios.Customers.Domain.Dtos.Customers;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Dtos.Responses;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Domain.Services;

namespace ClubeBeneficios.Customers.Infrastructure.Services;

public sealed class PartnerCustomerService : IPartnerCustomerService
{
    private readonly IPartnerCustomerRepository _partnerCustomerRepository;

    public PartnerCustomerService(IPartnerCustomerRepository partnerCustomerRepository)
    {
        _partnerCustomerRepository = partnerCustomerRepository;
    }

    public Task<PagedResultDto<PartnerCustomerListItemDto>> GetPagedAsync(PartnerCustomerFilterDto filter, CancellationToken cancellationToken = default)
        => _partnerCustomerRepository.GetPagedAsync(filter, cancellationToken);

    public Task<PartnerCustomerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _partnerCustomerRepository.GetByIdAsync(id, cancellationToken);

    public Task<Guid> CreateAsync(CreatePartnerCustomerRequest request, CancellationToken cancellationToken = default)
        => _partnerCustomerRepository.CreateAsync(request, cancellationToken);

    public Task<bool> UpdateAsync(Guid id, UpdatePartnerCustomerRequest request, CancellationToken cancellationToken = default)
        => _partnerCustomerRepository.UpdateAsync(id, request, cancellationToken);
}