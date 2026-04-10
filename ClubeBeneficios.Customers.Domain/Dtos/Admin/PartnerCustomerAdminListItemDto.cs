namespace ClubeBeneficios.Customers.Domain.Dtos.Admin;

public sealed class PartnerCustomerAdminListItemDto
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
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int Rn { get; set; }
    public int TotalCount { get; set; }
}