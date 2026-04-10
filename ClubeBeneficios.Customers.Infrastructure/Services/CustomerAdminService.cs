using ClubeBeneficios.Customers.Domain.Dtos.Admin;
using ClubeBeneficios.Customers.Domain.Dtos.Common;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Interfaces;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Domain.Services;

namespace ClubeBeneficios.Customers.Infrastructure.Services;

public sealed class CustomerAdminService : ICustomerAdminService
{
    private readonly ICustomerAdminRepository _repository;
    private readonly IUserContext _userContext;

    public CustomerAdminService(ICustomerAdminRepository repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public Task<PagedResultDto<CustomerAdminListItemDto>> SearchPagedAsync(CustomerAdminFilterDto filter, CancellationToken cancellationToken = default)
        => _repository.SearchPagedAsync(filter, cancellationToken);

    public Task<CustomerAdminSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
        => _repository.GetDashboardSummaryAsync(cancellationToken);

    public Task<IReadOnlyCollection<FilterOptionDto>> GetFilterOptionsAsync(CancellationToken cancellationToken = default)
        => _repository.GetFilterOptionsAsync(cancellationToken);

    public Task<CustomerAdminDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(id, cancellationToken);

    public Task<Guid> CreateAsync(CreateCustomerAdminRequest request, CancellationToken cancellationToken = default)
        => _repository.CreateAsync(request, GetCurrentUserId(), cancellationToken);

    public Task<bool> UpdateAsync(Guid id, UpdateCustomerAdminRequest request, CancellationToken cancellationToken = default)
        => _repository.UpdateAsync(id, request, GetCurrentUserId(), cancellationToken);

    public Task<IReadOnlyCollection<PetDto>> GetPetsAsync(Guid clientId, CancellationToken cancellationToken = default)
        => _repository.GetPetsAsync(clientId, cancellationToken);

    public Task<Guid> AddPetAsync(Guid clientId, CreatePetRequest request, CancellationToken cancellationToken = default)
        => _repository.AddPetAsync(clientId, request, cancellationToken);

    public Task<bool> UpdatePetAsync(Guid clientId, Guid petId, UpdatePetRequest request, CancellationToken cancellationToken = default)
        => _repository.UpdatePetAsync(clientId, petId, request, cancellationToken);

    public Task<IReadOnlyCollection<DocumentDto>> GetDocumentsAsync(Guid clientId, CancellationToken cancellationToken = default)
        => _repository.GetDocumentsAsync(clientId, cancellationToken);

    public Task<Guid> AddDocumentAsync(Guid clientId, AddDocumentRequest request, CancellationToken cancellationToken = default)
        => _repository.AddDocumentAsync(clientId, request, cancellationToken);

    public Task<IReadOnlyCollection<NoteDto>> GetNotesAsync(Guid clientId, CancellationToken cancellationToken = default)
        => _repository.GetNotesAsync(clientId, cancellationToken);

    public Task<Guid> AddNoteAsync(Guid clientId, AddNoteRequest request, CancellationToken cancellationToken = default)
        => _repository.AddNoteAsync(clientId, request, GetCurrentUserId(), cancellationToken);

    public Task<IReadOnlyCollection<StatusHistoryDto>> GetStatusHistoryAsync(Guid clientId, CancellationToken cancellationToken = default)
        => _repository.GetStatusHistoryAsync(clientId, cancellationToken);

    public Task<bool> UpdateStatusAsync(Guid clientId, UpdateClientStatusRequest request, CancellationToken cancellationToken = default)
        => _repository.UpdateStatusAsync(clientId, request, GetCurrentUserId(), cancellationToken);

    private Guid? GetCurrentUserId()
        => Guid.TryParse(_userContext.UserId, out var value) ? value : null;
}