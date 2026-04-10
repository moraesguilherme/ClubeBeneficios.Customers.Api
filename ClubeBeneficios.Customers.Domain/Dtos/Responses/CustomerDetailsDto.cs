namespace ClubeBeneficios.Customers.Domain.Dtos.Responses;

public sealed class CustomerDetailsDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Level { get; set; }
    public string? Notes { get; set; }
    public int PetsCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}