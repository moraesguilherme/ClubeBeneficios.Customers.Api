using ClubeBeneficios.Customers.Domain.Dtos;
using ClubeBeneficios.Customers.Domain.Dtos.Customers;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Dtos.Responses;

namespace ClubeBeneficios.Customers.Domain.Services;

public interface IPartnerCustomerService
{
    Task<PagedResultDto<PartnerCustomerListItemDto>> GetPagedAsync(PartnerCustomerFilterDto filter, CancellationToken cancellationToken = default);
    Task<PartnerCustomerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreatePartnerCustomerRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, UpdatePartnerCustomerRequest request, CancellationToken cancellationToken = default);
}