namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class UpdateClientStatusRequest
{
    public string NewStatus { get; set; } = string.Empty;
    public string? Reason { get; set; }
}