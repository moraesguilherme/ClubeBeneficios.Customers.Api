using ClubeBeneficios.Customers.Domain.Dtos;
using ClubeBeneficios.Customers.Domain.Dtos.Customers;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Dtos.Responses;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Infrastructure.Context;

namespace ClubeBeneficios.Customers.Infrastructure.Repositories;

public sealed class PartnerCustomerRepository : IPartnerCustomerRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PartnerCustomerRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public Task<PagedResultDto<PartnerCustomerListItemDto>> GetPagedAsync(PartnerCustomerFilterDto filter, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("ImplementaÃ§Ã£o serÃ¡ feita no prÃ³ximo bloco com queries/views/procedures do banco.");
    }

    public Task<PartnerCustomerDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("ImplementaÃ§Ã£o serÃ¡ feita no prÃ³ximo bloco com queries/views/procedures do banco.");
    }

    public Task<Guid> CreateAsync(CreatePartnerCustomerRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("ImplementaÃ§Ã£o serÃ¡ feita no prÃ³ximo bloco com procedures de criaÃ§Ã£o.");
    }

    public Task<bool> UpdateAsync(Guid id, UpdatePartnerCustomerRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("ImplementaÃ§Ã£o serÃ¡ feita no prÃ³ximo bloco com procedures de atualizaÃ§Ã£o.");
    }
}