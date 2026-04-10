using ClubeBeneficios.Customers.Domain.Dtos.Admin;
using ClubeBeneficios.Customers.Domain.Dtos.Common;
using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Repositories;
using ClubeBeneficios.Customers.Infrastructure.Context;
using Dapper;
using System.Data;

namespace ClubeBeneficios.Customers.Infrastructure.Repositories;

public sealed class PartnerCustomerAdminRepository : IPartnerCustomerAdminRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public PartnerCustomerAdminRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<PagedResultDto<PartnerCustomerAdminListItemDto>> SearchPagedAsync(PartnerCustomerAdminFilterDto filter, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var parameters = new DynamicParameters();
        parameters.Add("Search", NullIfWhiteSpace(filter.Search));
        parameters.Add("PartnerId", filter.PartnerId);
        parameters.Add("Status", NormalizeOptionalFilter(filter.Status));
        parameters.Add("RegistrationStage", NormalizeOptionalFilter(filter.RegistrationStage));
        parameters.Add("PageNumber", filter.PageNumber <= 0 ? 1 : filter.PageNumber);
        parameters.Add("PageSize", NormalizePageSize(filter.PageSize));

        var items = (await connection.QueryAsync<PartnerCustomerAdminListItemDto>(
            new CommandDefinition(
                "dbo.usp_partner_customers_admin_search_paged",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken)))
            .ToList();

        var totalCount = items.FirstOrDefault()?.TotalCount ?? 0;

        return new PagedResultDto<PartnerCustomerAdminListItemDto>
        {
            Items = items,
            Page = filter.PageNumber <= 0 ? 1 : filter.PageNumber,
            PageSize = NormalizePageSize(filter.PageSize),
            TotalCount = totalCount
        };
    }

    public async Task<PartnerCustomerAdminDashboardDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        using var multi = await connection.QueryMultipleAsync(
            new CommandDefinition(
                "dbo.usp_partner_customers_admin_dashboard_summary",
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        var summary = await multi.ReadFirstAsync<PartnerCustomerAdminSummaryDto>();
        var funnel = (await multi.ReadAsync<PartnerCustomersConversionFunnelDto>()).ToList();

        return new PartnerCustomerAdminDashboardDto
        {
            Summary = summary,
            ConversionFunnels = funnel
        };
    }

    public async Task<IReadOnlyCollection<FilterOptionDto>> GetFilterOptionsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var result = await connection.QueryAsync<FilterOptionDto>(
            new CommandDefinition(
                "dbo.usp_partner_customers_filter_options",
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<IReadOnlyCollection<PartnerOptionDto>> GetPartnerFilterOptionsAsync(CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        var result = await connection.QueryAsync<PartnerOptionDto>(
            new CommandDefinition(
                "dbo.usp_partner_customers_partner_filter_options",
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<PartnerCustomerAdminDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    pc.id,
    pc.user_id,
    pc.partner_id,
    p.trade_name AS partner_name,
    pc.access_code_id,
    pc.full_name,
    pc.email,
    pc.phone,
    pc.document,
    pc.birth_date,
    pc.origin_type,
    pc.origin_channel,
    pc.registration_stage,
    pc.status,
    pc.benefits_dashboard_unlocked_at,
    pc.converted_to_full_registration_at,
    pc.first_access_at,
    pc.last_access_at,
    pc.accepted_terms_at,
    pc.accepted_privacy_policy_at,
    pc.notes_summary,
    pc.created_at,
    pc.updated_at,
    pc.created_by_user_id,
    pc.updated_by_user_id
FROM dbo.partner_customers pc
INNER JOIN dbo.partners p
    ON p.id = pc.partner_id
WHERE pc.id = @Id;";

        var details = await connection.QueryFirstOrDefaultAsync<PartnerCustomerAdminDetailsDto>(
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

    public async Task<Guid> CreateAsync(CreatePartnerCustomerAdminRequest request, Guid? performedByUserId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
INSERT INTO dbo.partner_customers
(
    user_id,
    partner_id,
    access_code_id,
    full_name,
    email,
    phone,
    document,
    birth_date,
    origin_type,
    origin_channel,
    registration_stage,
    status,
    benefits_dashboard_unlocked_at,
    converted_to_full_registration_at,
    first_access_at,
    last_access_at,
    accepted_terms_at,
    accepted_privacy_policy_at,
    notes_summary,
    created_by_user_id,
    updated_by_user_id
)
OUTPUT INSERTED.id
VALUES
(
    @UserId,
    @PartnerId,
    @AccessCodeId,
    @FullName,
    @Email,
    @Phone,
    @Document,
    @BirthDate,
    @OriginType,
    @OriginChannel,
    @RegistrationStage,
    @Status,
    @BenefitsDashboardUnlockedAt,
    @ConvertedToFullRegistrationAt,
    @FirstAccessAt,
    @LastAccessAt,
    @AcceptedTermsAt,
    @AcceptedPrivacyPolicyAt,
    @NotesSummary,
    @PerformedByUserId,
    @PerformedByUserId
);";

        return await connection.ExecuteScalarAsync<Guid>(
            new CommandDefinition(
                sql,
                new
                {
                    request.UserId,
                    request.PartnerId,
                    request.AccessCodeId,
                    request.FullName,
                    request.Email,
                    request.Phone,
                    request.Document,
                    request.BirthDate,
                    request.OriginType,
                    request.OriginChannel,
                    request.RegistrationStage,
                    request.Status,
                    request.BenefitsDashboardUnlockedAt,
                    request.ConvertedToFullRegistrationAt,
                    request.FirstAccessAt,
                    request.LastAccessAt,
                    request.AcceptedTermsAt,
                    request.AcceptedPrivacyPolicyAt,
                    request.NotesSummary,
                    PerformedByUserId = performedByUserId
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));
    }

    public async Task<bool> UpdateAsync(Guid id, UpdatePartnerCustomerAdminRequest request, Guid? performedByUserId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
UPDATE dbo.partner_customers
SET
    user_id = @UserId,
    partner_id = @PartnerId,
    access_code_id = @AccessCodeId,
    full_name = @FullName,
    email = @Email,
    phone = @Phone,
    document = @Document,
    birth_date = @BirthDate,
    origin_type = @OriginType,
    origin_channel = @OriginChannel,
    registration_stage = @RegistrationStage,
    status = @Status,
    benefits_dashboard_unlocked_at = @BenefitsDashboardUnlockedAt,
    converted_to_full_registration_at = @ConvertedToFullRegistrationAt,
    first_access_at = @FirstAccessAt,
    last_access_at = @LastAccessAt,
    accepted_terms_at = @AcceptedTermsAt,
    accepted_privacy_policy_at = @AcceptedPrivacyPolicyAt,
    notes_summary = @NotesSummary,
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
                    request.PartnerId,
                    request.AccessCodeId,
                    request.FullName,
                    request.Email,
                    request.Phone,
                    request.Document,
                    request.BirthDate,
                    request.OriginType,
                    request.OriginChannel,
                    request.RegistrationStage,
                    request.Status,
                    request.BenefitsDashboardUnlockedAt,
                    request.ConvertedToFullRegistrationAt,
                    request.FirstAccessAt,
                    request.LastAccessAt,
                    request.AcceptedTermsAt,
                    request.AcceptedPrivacyPolicyAt,
                    request.NotesSummary,
                    PerformedByUserId = performedByUserId
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));

        return rows > 0;
    }

    public async Task<IReadOnlyCollection<PetDto>> GetPetsAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    p.id,
    p.partner_customer_id AS owner_id,
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
FROM dbo.partner_customer_pets p
WHERE p.partner_customer_id = @PartnerCustomerId
ORDER BY p.created_at DESC, p.name ASC;";

        var result = await connection.QueryAsync<PetDto>(
            new CommandDefinition(sql, new { PartnerCustomerId = partnerCustomerId }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<Guid> AddPetAsync(Guid partnerCustomerId, CreatePetRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
INSERT INTO dbo.partner_customer_pets
(
    partner_customer_id,
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
    @PartnerCustomerId,
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
                    PartnerCustomerId = partnerCustomerId,
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

    public async Task<bool> UpdatePetAsync(Guid partnerCustomerId, Guid petId, UpdatePetRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
UPDATE dbo.partner_customer_pets
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
  AND partner_customer_id = @PartnerCustomerId;";

        var rows = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    PartnerCustomerId = partnerCustomerId,
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

    public async Task<IReadOnlyCollection<DocumentDto>> GetDocumentsAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    d.id,
    d.partner_customer_id AS owner_id,
    d.partner_customer_pet_id AS pet_id,
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
FROM dbo.partner_customer_documents d
WHERE d.partner_customer_id = @PartnerCustomerId
ORDER BY d.created_at DESC, d.file_name ASC;";

        var result = await connection.QueryAsync<DocumentDto>(
            new CommandDefinition(sql, new { PartnerCustomerId = partnerCustomerId }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<Guid> AddDocumentAsync(Guid partnerCustomerId, AddDocumentRequest request, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
INSERT INTO dbo.partner_customer_documents
(
    partner_customer_id,
    partner_customer_pet_id,
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
    @PartnerCustomerId,
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
                    PartnerCustomerId = partnerCustomerId,
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

    public async Task<IReadOnlyCollection<NoteDto>> GetNotesAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    n.id,
    n.partner_customer_id AS owner_id,
    n.note_type,
    n.title,
    n.content,
    n.is_internal,
    n.created_at,
    n.created_by_user_id
FROM dbo.partner_customer_notes n
WHERE n.partner_customer_id = @PartnerCustomerId
ORDER BY n.created_at DESC;";

        var result = await connection.QueryAsync<NoteDto>(
            new CommandDefinition(sql, new { PartnerCustomerId = partnerCustomerId }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<Guid> AddNoteAsync(Guid partnerCustomerId, AddNoteRequest request, Guid? createdByUserId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
INSERT INTO dbo.partner_customer_notes
(
    partner_customer_id,
    note_type,
    title,
    content,
    is_internal,
    created_by_user_id
)
OUTPUT INSERTED.id
VALUES
(
    @PartnerCustomerId,
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
                    PartnerCustomerId = partnerCustomerId,
                    request.NoteType,
                    request.Title,
                    request.Content,
                    request.IsInternal,
                    CreatedByUserId = createdByUserId
                },
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<StatusHistoryDto>> GetStatusHistoryAsync(Guid partnerCustomerId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        const string sql = @"
SELECT
    h.id,
    h.partner_customer_id AS owner_id,
    h.old_status,
    h.new_status,
    h.old_registration_stage,
    h.new_registration_stage,
    h.reason,
    h.changed_at,
    h.changed_by_user_id
FROM dbo.partner_customer_status_history h
WHERE h.partner_customer_id = @PartnerCustomerId
ORDER BY h.changed_at DESC, h.id DESC;";

        var result = await connection.QueryAsync<StatusHistoryDto>(
            new CommandDefinition(sql, new { PartnerCustomerId = partnerCustomerId }, commandType: CommandType.Text, cancellationToken: cancellationToken));

        return result.ToList();
    }

    public async Task<bool> UpdateStatusAsync(Guid partnerCustomerId, UpdatePartnerCustomerStatusRequest request, Guid? changedByUserId, CancellationToken cancellationToken = default)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        const string getCurrentSql = @"
SELECT
    status AS Status,
    registration_stage AS RegistrationStage
FROM dbo.partner_customers
WHERE id = @PartnerCustomerId;";

        var current = await connection.QueryFirstOrDefaultAsync<CurrentPartnerCustomerState>(
            new CommandDefinition(
                getCurrentSql,
                new { PartnerCustomerId = partnerCustomerId },
                transaction: transaction,
                commandType: CommandType.Text,
                cancellationToken: cancellationToken));

        if (current is null)
        {
            transaction.Rollback();
            return false;
        }

        var newRegistrationStage = string.IsNullOrWhiteSpace(request.NewRegistrationStage)
            ? current.RegistrationStage
            : request.NewRegistrationStage;

        const string updateSql = @"
UPDATE dbo.partner_customers
SET
    status = @NewStatus,
    registration_stage = @NewRegistrationStage,
    updated_at = SYSUTCDATETIME(),
    updated_by_user_id = @ChangedByUserId
WHERE id = @PartnerCustomerId;";

        var rows = await connection.ExecuteAsync(
            new CommandDefinition(
                updateSql,
                new
                {
                    PartnerCustomerId = partnerCustomerId,
                    request.NewStatus,
                    NewRegistrationStage = newRegistrationStage,
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
INSERT INTO dbo.partner_customer_status_history
(
    partner_customer_id,
    old_status,
    new_status,
    old_registration_stage,
    new_registration_stage,
    reason,
    changed_by_user_id
)
VALUES
(
    @PartnerCustomerId,
    @OldStatus,
    @NewStatus,
    @OldRegistrationStage,
    @NewRegistrationStage,
    @Reason,
    @ChangedByUserId
);";

        await connection.ExecuteAsync(
            new CommandDefinition(
                insertHistorySql,
                new
                {
                    PartnerCustomerId = partnerCustomerId,
                    OldStatus = current.Status,
                    request.NewStatus,
                    OldRegistrationStage = current.RegistrationStage,
                    NewRegistrationStage = newRegistrationStage,
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

    private sealed class CurrentPartnerCustomerState
    {
        public string Status { get; set; } = string.Empty;
        public string RegistrationStage { get; set; } = string.Empty;
    }
}