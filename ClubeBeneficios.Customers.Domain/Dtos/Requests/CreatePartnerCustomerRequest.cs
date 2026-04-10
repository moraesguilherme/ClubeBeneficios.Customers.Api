namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class CreatePartnerCustomerRequest
{
    public Guid PartnerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? OriginType { get; set; }
    public string? RegistrationStage { get; set; }
}