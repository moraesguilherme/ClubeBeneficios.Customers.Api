using ClubeBeneficios.Customers.Domain.Dtos.Admin;
using ClubeBeneficios.Customers.Domain.Dtos.Common;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;

namespace ClubeBeneficios.Customers.Domain.Repositories;

public interface ICustomerAdminRepository
{
    Task<PagedResultDto<CustomerAdminListItemDto>> SearchPagedAsync(CustomerAdminFilterDto filter, CancellationToken cancellationToken = default);
    Task<CustomerAdminSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<FilterOptionDto>> GetFilterOptionsAsync(CancellationToken cancellationToken = default);

    Task<CustomerAdminDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(CreateCustomerAdminRequest request, Guid? performedByUserId, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Guid id, UpdateCustomerAdminRequest request, Guid? performedByUserId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<PetDto>> GetPetsAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<Guid> AddPetAsync(Guid clientId, CreatePetRequest request, CancellationToken cancellationToken = default);
    Task<bool> UpdatePetAsync(Guid clientId, Guid petId, UpdatePetRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<DocumentDto>> GetDocumentsAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<Guid> AddDocumentAsync(Guid clientId, AddDocumentRequest request, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<NoteDto>> GetNotesAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<Guid> AddNoteAsync(Guid clientId, AddNoteRequest request, Guid? createdByUserId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<StatusHistoryDto>> GetStatusHistoryAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<bool> UpdateStatusAsync(Guid clientId, UpdateClientStatusRequest request, Guid? changedByUserId, CancellationToken cancellationToken = default);
}