using ClubeBeneficios.Customers.Domain.Dtos;
using ClubeBeneficios.Customers.Domain.Dtos.Customers;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Dtos.Responses;

namespace ClubeBeneficios.Customers.Domain.Services;

public interface ICustomerService
{
    Task<PagedResultDto<CustomerListItemDto>> GetPagedAsync(CustomerFilterDto filter, CancellationToken cancellationToken = default);
    Task<CustomerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default);
}