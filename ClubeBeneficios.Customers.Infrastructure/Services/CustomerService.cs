using ClubeBeneficios.Customers.Domain.Dtos;
using ClubeBeneficios.Customers.Domain.Dtos.Customers;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Dtos.Responses;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Domain.Services;

namespace ClubeBeneficios.Customers.Infrastructure.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public Task<PagedResultDto<CustomerListItemDto>> GetPagedAsync(CustomerFilterDto filter, CancellationToken cancellationToken = default)
        => _customerRepository.GetPagedAsync(filter, cancellationToken);

    public Task<CustomerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _customerRepository.GetByIdAsync(id, cancellationToken);

    public Task<Guid> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
        => _customerRepository.CreateAsync(request, cancellationToken);

    public Task<bool> UpdateAsync(Guid id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
        => _customerRepository.UpdateAsync(id, request, cancellationToken);
}