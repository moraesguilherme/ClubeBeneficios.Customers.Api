namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class UpdatePartnerCustomerAdminRequest
{
    public Guid? UserId { get; set; }
    public Guid PartnerId { get; set; }
    public Guid? AccessCodeId { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Document { get; set; }
    public DateTime? BirthDate { get; set; }
    public string OriginType { get; set; } = "manual";
    public string OriginChannel { get; set; } = "internal";
    public string RegistrationStage { get; set; } = "pre_registered";
    public string Status { get; set; } = "active";
    public DateTime? BenefitsDashboardUnlockedAt { get; set; }
    public DateTime? ConvertedToFullRegistrationAt { get; set; }
    public DateTime? FirstAccessAt { get; set; }
    public DateTime? LastAccessAt { get; set; }
    public DateTime? AcceptedTermsAt { get; set; }
    public DateTime? AcceptedPrivacyPolicyAt { get; set; }
    public string? NotesSummary { get; set; }
}