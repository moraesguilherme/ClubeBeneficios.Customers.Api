using ClubeBeneficios.Customers.Domain.Dtos.Admin;
using ClubeBeneficios.Customers.Domain.Dtos.Common;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Infrastructure.Context;
using Dapper;
using System.Data;

namespace ClubeBeneficios.Customers.Infrastructure.Repositories;

public sealed class CustomerAdminRepository : ICustomerAdminRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public CustomerAdminRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PagedResultDto<CustomerAdminListItemDto>> SearchPagedAsync(CustomerAdminFilterDto filter, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("Search", NullIfWhiteSpace(filter.Search));
        parameters.Add("Status", NormalizeOptionalFilter(filter.Status));
        parameters.Add("OriginType", NormalizeOptionalFilter(filter.OriginType));
        parameters.Add("PageNumber", filter.PageNumber <= 0 ? 1 : filter.PageNumber);
        parameters.Add("PageSize", NormalizePageSize(filter.PageSize));

        var items = (await connection.QueryAsync<CustomerAdminListItemDto>(
            new CommandDefinition(
                "dbo.usp_clients_admin_search_paged",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken)))
            .ToList();

        var totalCount = items.FirstOrDefault()?.TotalCount ?? 0;

        return new PagedResultDto<CustomerAdminListItemDto>
        {
            Items = items,
            Page = filter.PageNumber <= 0 ? 1 : filter.PageNumber,
            PageSize = NormalizePageSize(filter.PageSize),
            TotalCount = totalCount
        };
    }

    public async Task<CustomerAdminSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        return await connection.QueryFirstAsync<CustomerAdminSummaryDto>(
            new CommandDefinition(
                "dbo.usp_clients_admin_dashboard_summary",
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<FilterOptionDto>> GetFilterOptionsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var result = await connection.QueryAsync<FilterOptionDto>(
            new CommandDefinition(
                "dbo.usp_clients_filter_options",
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<CustomerAdminDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    c.id,
    c.user_id,
    c.full_name,
    c.document,
    c.email,
    c.phone,
    c.birth_date,
    c.instagram,
    c.address_zip_code,
    c.address_street,
    c.address_number,
    c.address_complement,
    c.address_neighborhood,
    c.address_city,
    c.address_state,
    c.origin_type,
    c.status,
    c.notes_summary,
    c.accepts_marketing,
    c.lgpd_consent_at,
    c.first_service_at,
    c.last_service_at,
    c.created_at,
    c.updated_at,
    c.created_by_user_id,
    c.updated_by_user_id
FROM dbo.clients c
WHERE c.id = @Id;";

        var details = await connection.QueryFirstOrDefaultAsync<CustomerAdminDetailsDto>(
            new CommandDefinition(sql, new { Id = id }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        if (details is null)
        {
            return null;
        }

        details.Pets = await GetPetsAsync(id, cancellationToken);
        details.Documents = await GetDocumentsAsync(id, cancellationToken);
        details.Notes = await GetNotesAsync(id, cancellationToken);
        details.StatusHistory = await GetStatusHistoryAsync(id, cancellationToken);

        return details;
    }

    public async Task<Guid> CreateAsync(CreateCustomerAdminRequest request, Guid? performedByUserId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
INSERT INTO dbo.clients
(
    user_id,
    full_name,
    document,
    email,
    phone,
    birth_date,
    instagram,
    address_zip_code,
    address_street,
    address_number,
    address_complement,
    address_neighborhood,
    address_city,
    address_state,
    origin_type,
    status,
    notes_summary,
    accepts_marketing,
    lgpd_consent_at,
    first_service_at,
    last_service_at,
    created_by_user_id,
    updated_by_user_id
)
OUTPUT INSERTED.id
VALUES
(
    @UserId,
    @FullName,
    @Document,
    @Email,
    @Phone,
    @BirthDate,
    @Instagram,
    @AddressZipCode,
    @AddressStreet,
    @AddressNumber,
    @AddressComplement,
    @AddressNeighborhood,
    @AddressCity,
    @AddressState,
    @OriginType,
    @Status,
    @NotesSummary,
    @AcceptsMarketing,
    @LgpdConsentAt,
    @FirstServiceAt,
    @LastServiceAt,
    @PerformedByUserId,
    @PerformedByUserId
);";

        return await connection.ExecuteScalarAsync<Guid>(
            new CommandDefinition(
                sql,
                new
                {
                    request.UserId,
                    request.FullName,
                    request.Document,
                    request.Email,
                    request.Phone,
                    request.BirthDate,
                    request.Instagram,
                    request.AddressZipCode,
                    request.AddressStreet,
                    request.AddressNumber,
                    request.AddressComplement,
                    request.AddressNeighborhood,
                    request.AddressCity,
                    request.AddressState,
                    request.OriginType,
                    request.Status,
                    request.NotesSummary,
                    request.AcceptsMarketing,
                    request.LgpdConsentAt,
                    request.FirstServiceAt,
                    request.LastServiceAt,
                    PerformedByUserId = performedByUserId
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateCustomerAdminRequest request, Guid? performedByUserId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
UPDATE dbo.clients
SET
    user_id = @UserId,
    full_name = @FullName,
    document = @Document,
    email = @Email,
    phone = @Phone,
    birth_date = @BirthDate,
    instagram = @Instagram,
    address_zip_code = @AddressZipCode,
    address_street = @AddressStreet,
    address_number = @AddressNumber,
    address_complement = @AddressComplement,
    address_neighborhood = @AddressNeighborhood,
    address_city = @AddressCity,
    address_state = @AddressState,
    origin_type = @OriginType,
    status = @Status,
    notes_summary = @NotesSummary,
    accepts_marketing = @AcceptsMarketing,
    lgpd_consent_at = @LgpdConsentAt,
    first_service_at = @FirstServiceAt,
    last_service_at = @LastServiceAt,
    updated_at = SYSUTCDATETIME(),
    updated_by_user_id = @PerformedByUserId
WHERE id = @Id;";

        var rows = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    Id = id,
                    request.UserId,
                    request.FullName,
                    request.Document,
                    request.Email,
                    request.Phone,
                    request.BirthDate,
                    request.Instagram,
                    request.AddressZipCode,
                    request.AddressStreet,
                    request.AddressNumber,
                    request.AddressComplement,
                    request.AddressNeighborhood,
                    request.AddressCity,
                    request.AddressState,
                    request.OriginType,
                    request.Status,
                    request.NotesSummary,
                    request.AcceptsMarketing,
                    request.LgpdConsentAt,
                    request.FirstServiceAt,
                    request.LastServiceAt,
                    PerformedByUserId = performedByUserId
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));

        return rows > 0;
    }

    public async Task<IReadOnlyCollection<PetDto>> GetPetsAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    p.id,
    p.client_id AS owner_id,
    p.name,
    p.species,
    p.breed,
    p.sex,
    p.birth_date,
    p.age_months,
    p.weight_kg,
    p.size,
    p.color,
    p.is_neutered,
    p.neutered_at,
    p.behavior_status,
    p.temperament_summary,
    p.restriction_notes,
    p.medical_notes,
    p.feeding_notes,
    p.special_care_notes,
    p.status,
    p.created_at,
    p.updated_at
FROM dbo.client_pets p
WHERE p.client_id = @ClientId
ORDER BY p.created_at DESC, p.name ASC;";

        var result = await connection.QueryAsync<PetDto>(
            new CommandDefinition(sql, new { ClientId = clientId }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<Guid> AddPetAsync(Guid clientId, CreatePetRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
INSERT INTO dbo.client_pets
(
    client_id,
    name,
    species,
    breed,
    sex,
    birth_date,
    age_months,
    weight_kg,
    size,
    color,
    is_neutered,
    neutered_at,
    behavior_status,
    temperament_summary,
    restriction_notes,
    medical_notes,
    feeding_notes,
    special_care_notes,
    status
)
OUTPUT INSERTED.id
VALUES
(
    @ClientId,
    @Name,
    @Species,
    @Breed,
    @Sex,
    @BirthDate,
    @AgeMonths,
    @WeightKg,
    @Size,
    @Color,
    @IsNeutered,
    @NeuteredAt,
    @BehaviorStatus,
    @TemperamentSummary,
    @RestrictionNotes,
    @MedicalNotes,
    @FeedingNotes,
    @SpecialCareNotes,
    @Status
);";

        return await connection.ExecuteScalarAsync<Guid>(
            new CommandDefinition(
                sql,
                new
                {
                    ClientId = clientId,
                    request.Name,
                    request.Species,
                    request.Breed,
                    request.Sex,
                    request.BirthDate,
                    request.AgeMonths,
                    request.WeightKg,
                    request.Size,
                    request.Color,
                    request.IsNeutered,
                    request.NeuteredAt,
                    request.BehaviorStatus,
                    request.TemperamentSummary,
                    request.RestrictionNotes,
                    request.MedicalNotes,
                    request.FeedingNotes,
                    request.SpecialCareNotes,
                    request.Status
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdatePetAsync(Guid clientId, Guid petId, UpdatePetRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
UPDATE dbo.client_pets
SET
    name = @Name,
    species = @Species,
    breed = @Breed,
    sex = @Sex,
    birth_date = @BirthDate,
    age_months = @AgeMonths,
    weight_kg = @WeightKg,
    size = @Size,
    color = @Color,
    is_neutered = @IsNeutered,
    neutered_at = @NeuteredAt,
    behavior_status = @BehaviorStatus,
    temperament_summary = @TemperamentSummary,
    restriction_notes = @RestrictionNotes,
    medical_notes = @MedicalNotes,
    feeding_notes = @FeedingNotes,
    special_care_notes = @SpecialCareNotes,
    status = @Status,
    updated_at = SYSUTCDATETIME()
WHERE id = @PetId
  AND client_id = @ClientId;";

        var rows = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    ClientId = clientId,
                    PetId = petId,
                    request.Name,
                    request.Species,
                    request.Breed,
                    request.Sex,
                    request.BirthDate,
                    request.AgeMonths,
                    request.WeightKg,
                    request.Size,
                    request.Color,
                    request.IsNeutered,
                    request.NeuteredAt,
                    request.BehaviorStatus,
                    request.TemperamentSummary,
                    request.RestrictionNotes,
                    request.MedicalNotes,
                    request.FeedingNotes,
                    request.SpecialCareNotes,
                    request.Status
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));

        return rows > 0;
    }

    public async Task<IReadOnlyCollection<DocumentDto>> GetDocumentsAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    d.id,
    d.client_id AS owner_id,
    d.client_pet_id AS pet_id,
    d.document_type,
    d.file_url,
    d.file_name,
    d.mime_type,
    d.status,
    d.expires_at,
    d.verified_at,
    d.verified_by_user_id,
    d.rejection_reason,
    d.created_at,
    d.updated_at
FROM dbo.client_documents d
WHERE d.client_id = @ClientId
ORDER BY d.created_at DESC, d.file_name ASC;";

        var result = await connection.QueryAsync<DocumentDto>(
            new CommandDefinition(sql, new { ClientId = clientId }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<Guid> AddDocumentAsync(Guid clientId, AddDocumentRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
INSERT INTO dbo.client_documents
(
    client_id,
    client_pet_id,
    document_type,
    file_url,
    file_name,
    mime_type,
    status,
    expires_at,
    verified_at,
    verified_by_user_id,
    rejection_reason
)
OUTPUT INSERTED.id
VALUES
(
    @ClientId,
    @PetId,
    @DocumentType,
    @FileUrl,
    @FileName,
    @MimeType,
    @Status,
    @ExpiresAt,
    @VerifiedAt,
    @VerifiedByUserId,
    @RejectionReason
);";

        return await connection.ExecuteScalarAsync<Guid>(
            new CommandDefinition(
                sql,
                new
                {
                    ClientId = clientId,
                    request.PetId,
                    request.DocumentType,
                    request.FileUrl,
                    request.FileName,
                    request.MimeType,
                    request.Status,
                    request.ExpiresAt,
                    request.VerifiedAt,
                    request.VerifiedByUserId,
                    request.RejectionReason
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<NoteDto>> GetNotesAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    n.id,
    n.client_id AS owner_id,
    n.note_type,
    n.title,
    n.content,
    n.is_internal,
    n.created_at,
    n.created_by_user_id
FROM dbo.client_notes n
WHERE n.client_id = @ClientId
ORDER BY n.created_at DESC;";

        var result = await connection.QueryAsync<NoteDto>(
            new CommandDefinition(sql, new { ClientId = clientId }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<Guid> AddNoteAsync(Guid clientId, AddNoteRequest request, Guid? createdByUserId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
INSERT INTO dbo.client_notes
(
    client_id,
    note_type,
    title,
    content,
    is_internal,
    created_by_user_id
)
OUTPUT INSERTED.id
VALUES
(
    @ClientId,
    @NoteType,
    @Title,
    @Content,
    @IsInternal,
    @CreatedByUserId
);";

        return await connection.ExecuteScalarAsync<Guid>(
            new CommandDefinition(
                sql,
                new
                {
                    ClientId = clientId,
                    request.NoteType,
                    request.Title,
                    request.Content,
                    request.IsInternal,
                    CreatedByUserId = createdByUserId
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<StatusHistoryDto>> GetStatusHistoryAsync(Guid clientId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    h.id,
    h.client_id AS owner_id,
    h.old_status,
    h.new_status,
    CAST(NULL AS VARCHAR(30)) AS old_registration_stage,
    CAST(NULL AS VARCHAR(30)) AS new_registration_stage,
    h.reason,
    h.changed_at,
    h.changed_by_user_id
FROM dbo.client_status_history h
WHERE h.client_id = @ClientId
ORDER BY h.changed_at DESC, h.id DESC;";

        var result = await connection.QueryAsync<StatusHistoryDto>(
            new CommandDefinition(sql, new { ClientId = clientId }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<bool> UpdateStatusAsync(Guid clientId, UpdateClientStatusRequest request, Guid? changedByUserId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        const string getCurrentStatusSql = @"SELECT status FROM dbo.clients WHERE id = @ClientId;";
        var currentStatus = await connection.QueryFirstOrDefaultAsync<string>(
            new CommandDefinition(
                getCurrentStatusSql,
                new { ClientId = clientId },
                transaction: transaction,
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));

        if (string.IsNullOrWhiteSpace(currentStatus))
        {
            transaction.Rollback();
            return false;
        }

        const string updateSql = @"
UPDATE dbo.clients
SET
    status = @NewStatus,
    updated_at = SYSUTCDATETIME(),
    updated_by_user_id = @ChangedByUserId
WHERE id = @ClientId;";

        var rows = await connection.ExecuteAsync(
            new CommandDefinition(
                updateSql,
                new
                {
                    ClientId = clientId,
                    request.NewStatus,
                    ChangedByUserId = changedByUserId
                },
                transaction: transaction,
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));

        if (rows <= 0)
        {
            transaction.Rollback();
            return false;
        }

        const string insertHistorySql = @"
INSERT INTO dbo.client_status_history
(
    client_id,
    old_status,
    new_status,
    reason,
    changed_by_user_id
)
VALUES
(
    @ClientId,
    @OldStatus,
    @NewStatus,
    @Reason,
    @ChangedByUserId
);";

        await connection.ExecuteAsync(
            new CommandDefinition(
                insertHistorySql,
                new
                {
                    ClientId = clientId,
                    OldStatus = currentStatus,
                    request.NewStatus,
                    request.Reason,
                    ChangedByUserId = changedByUserId
                },
                transaction: transaction,
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));

        transaction.Commit();
        return true;
    }

    private static int NormalizePageSize(int pageSize)
    {
        if (pageSize <= 0) return 20;
        if (pageSize > 200) return 200;
        return pageSize;
    }

    private static string? NormalizeOptionalFilter(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        return value.Trim();
    }

    private static string? NullIfWhiteSpace(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}