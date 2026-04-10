namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class UpdateCustomerRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Status { get; set; }
    public string? Level { get; set; }
}