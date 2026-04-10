namespace ClubeBeneficios.Customers.Domain.Interfaces;

public interface IUserContext
{
    string? UserId { get; set; }
    string? Email { get; set; }
    string? Role { get; set; }
    string? SessionId { get; set; }
    string? PartnerId { get; set; }
    string? Origin { get; set; }
}