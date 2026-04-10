namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class UpdatePartnerCustomerStatusRequest
{
    public string NewStatus { get; set; } = string.Empty;
    public string? NewRegistrationStage { get; set; }
    public string? Reason { get; set; }
}