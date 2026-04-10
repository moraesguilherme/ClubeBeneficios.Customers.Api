namespace ClubeBeneficios.Customers.Domain.Entities;

public sealed class PartnerCustomerEntity
{
    public Guid Id { get; set; }
    public Guid PartnerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = string.Empty;
    public string RegistrationStage { get; set; } = string.Empty;
    public string? OriginType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}