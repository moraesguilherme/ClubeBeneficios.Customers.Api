namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class RequestPartnerCustomerAdjustmentsRequest
{
    public string? Reason { get; set; }
    public string? PendingItems { get; set; }
    public string? Notes { get; set; }
}