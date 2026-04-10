namespace ClubeBeneficios.Customers.Domain.Entities;

public sealed class CustomerEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Document { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Level { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}