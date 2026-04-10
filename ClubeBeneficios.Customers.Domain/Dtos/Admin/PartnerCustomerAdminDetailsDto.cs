using ClubeBeneficios.Customers.Domain.Dtos.Common;

namespace ClubeBeneficios.Customers.Domain.Dtos.Admin;

public sealed class PartnerCustomerAdminDetailsDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid PartnerId { get; set; }
    public string PartnerName { get; set; } = string.Empty;
    public Guid? AccessCodeId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Document { get; set; }
    public DateTime? BirthDate { get; set; }
    public string OriginType { get; set; } = string.Empty;
    public string OriginChannel { get; set; } = string.Empty;
    public string RegistrationStage { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? BenefitsDashboardUnlockedAt { get; set; }
    public DateTime? ConvertedToFullRegistrationAt { get; set; }
    public DateTime? FirstAccessAt { get; set; }
    public DateTime? LastAccessAt { get; set; }
    public DateTime? AcceptedTermsAt { get; set; }
    public DateTime? AcceptedPrivacyPolicyAt { get; set; }
    public string? NotesSummary { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public IReadOnlyCollection<PetDto> Pets { get; set; } = Array.Empty<PetDto>();
    public IReadOnlyCollection<DocumentDto> Documents { get; set; } = Array.Empty<DocumentDto>();
    public IReadOnlyCollection<NoteDto> Notes { get; set; } = Array.Empty<NoteDto>();
    public IReadOnlyCollection<StatusHistoryDto> StatusHistory { get; set; } = Array.Empty<StatusHistoryDto>();
}