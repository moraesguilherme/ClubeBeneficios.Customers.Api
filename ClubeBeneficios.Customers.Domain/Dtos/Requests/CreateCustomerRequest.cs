namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class CreateCustomerRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Document { get; set; }
}