namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class ConvertPartnerCustomerResponse
{
    public Guid PartnerCustomerId { get; set; }
    public Guid? CustomerId { get; set; }
    public DateTime ConvertedAt { get; set; }
    public bool CustomerCreated { get; set; }
    public bool CustomerUpdated { get; set; }
    public string? Message { get; set; }
}