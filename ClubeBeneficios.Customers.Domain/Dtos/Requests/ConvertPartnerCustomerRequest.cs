namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class ConvertPartnerCustomerRequest
{
    public string? TargetCustomerStatus { get; set; }
    public string? Notes { get; set; }
    public bool? AcceptsMarketing { get; set; }
}