namespace ClubeBeneficios.Customers.Domain.Dtos.CustomerPortal;

public sealed class BenefitRequestListItemDto
{
    public Guid Id { get; set; }
    public Guid BenefitId { get; set; }
    public string BenefitTitle { get; set; } = string.Empty;
    public Guid? PartnerId { get; set; }
    public string? PartnerName { get; set; }
    public string? RequestStatus { get; set; }
    public string? ApprovalStatus { get; set; }
    public string? PetName { get; set; }
    public DateTime? RequestedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}