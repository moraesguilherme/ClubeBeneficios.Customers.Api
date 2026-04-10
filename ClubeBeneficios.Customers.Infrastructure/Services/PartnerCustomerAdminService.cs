using ClubeBeneficios.Customers.Domain.Dtos.Admin;
using ClubeBeneficios.Customers.Domain.Dtos.Common;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Interfaces;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Domain.Services;

namespace ClubeBeneficios.Customers.Infrastructure.Services;

public sealed class PartnerCustomerAdminService : IPartnerCustomerAdminService
{
    private readonly IPartnerCustomerAdminRepository _repository;
    private readonly IUserContext _userContext;

    public PartnerCustomerAdminService(IPartnerCustomerAdminRepository repository, IUserContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
    }

    public Task<PagedResultDto<PartnerCustomerAdminListItemDto>> SearchPagedAsync(PartnerCustomerAdminFilterDto filter, CancellationToken cancellationToken = default)
        => _repository.SearchPagedAsync(filter, cancellationToken);

    public Task<PartnerCustomerAdminDashboardDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
        => _repository.GetDashboardSummaryAsync(cancellationToken);

    public Task<IReadOnlyCollection<FilterOptionDto>> GetFilterOptionsAsync(CancellationToken cancellationToken = default)
        => _repository.GetFilterOptionsAsync(cancellationToken);

    public Task<IReadOnlyCollection<PartnerOptionDto>> GetPartnerFilterOptionsAsync(CancellationToken cancellationToken = default)
        => _repository.GetPartnerFilterOptionsAsync(cancellationToken);

    public Task<PartnerCustomerAdminDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.GetByIdAsync(id, cancellationToken);

    public Task<Guid> CreateAsync(CreatePartnerCustomerAdminRequest request, CancellationToken cancellationToken = default)
        => _repository.CreateAsync(request, GetCurrentUserId(), cancellationToken);

    public Task<bool> UpdateAsync(Guid id, UpdatePartnerCustomerAdminRequest request, CancellationToken cancellationToken = default)
        => _repository.UpdateAsync(id, request, GetCurrentUserId(), cancellationToken);

    public Task<IReadOnlyCollection<PetDto>> GetPetsAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
        => _repository.GetPetsAsync(partnerCustomerId, cancellationToken);

    public Task<Guid> AddPetAsync(Guid partnerCustomerId, CreatePetRequest request, CancellationToken cancellationToken = default)
        => _repository.AddPetAsync(partnerCustomerId, request, cancellationToken);

    public Task<bool> UpdatePetAsync(Guid partnerCustomerId, Guid petId, UpdatePetRequest request, CancellationToken cancellationToken = default)
        => _repository.UpdatePetAsync(partnerCustomerId, petId, request, cancellationToken);

    public Task<IReadOnlyCollection<DocumentDto>> GetDocumentsAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
        => _repository.GetDocumentsAsync(partnerCustomerId, cancellationToken);

    public Task<Guid> AddDocumentAsync(Guid partnerCustomerId, AddDocumentRequest request, CancellationToken cancellationToken = default)
        => _repository.AddDocumentAsync(partnerCustomerId, request, cancellationToken);

    public Task<IReadOnlyCollection<NoteDto>> GetNotesAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
        => _repository.GetNotesAsync(partnerCustomerId, cancellationToken);

    public Task<Guid> AddNoteAsync(Guid partnerCustomerId, AddNoteRequest request, CancellationToken cancellationToken = default)
        => _repository.AddNoteAsync(partnerCustomerId, request, GetCurrentUserId(), cancellationToken);

    public Task<IReadOnlyCollection<StatusHistoryDto>> GetStatusHistoryAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
        => _repository.GetStatusHistoryAsync(partnerCustomerId, cancellationToken);

    public Task<bool> UpdateStatusAsync(Guid partnerCustomerId, UpdatePartnerCustomerStatusRequest request, CancellationToken cancellationToken = default)
        => _repository.UpdateStatusAsync(partnerCustomerId, request, GetCurrentUserId(), cancellationToken);

    private Guid? GetCurrentUserId()
        => Guid.TryParse(_userContext.UserId, out var value) ? value : null;
}