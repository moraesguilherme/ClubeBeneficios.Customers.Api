namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class RejectPartnerCustomerReviewRequest
{
    public string? Reason { get; set; }
    public string? Notes { get; set; }
}