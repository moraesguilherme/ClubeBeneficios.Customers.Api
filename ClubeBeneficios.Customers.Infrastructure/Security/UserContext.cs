using ClubeBeneficios.Customers.Domain.Interfaces;

namespace ClubeBeneficios.Customers.Infrastructure.Security;

public sealed class UserContext : IUserContext
{
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public string? SessionId { get; set; }
    public string? PartnerId { get; set; }
    public string? Origin { get; set; }
}