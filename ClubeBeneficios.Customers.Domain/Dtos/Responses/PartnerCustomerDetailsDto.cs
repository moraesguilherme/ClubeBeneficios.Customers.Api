namespace ClubeBeneficios.Customers.Domain.Dtos.Responses;

public sealed class PartnerCustomerDetailsDto
{
    public Guid Id { get; set; }
    public Guid PartnerId { get; set; }
    public string PartnerName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = string.Empty;
    public string RegistrationStage { get; set; } = string.Empty;
    public string? OriginType { get; set; }
    public string? Notes { get; set; }
    public int PetsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}