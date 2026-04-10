namespace ClubeBeneficios.Customers.Domain.Dtos.Admin;

public sealed class CustomerAdminListItemDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Document { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Instagram { get; set; }
    public string? AddressZipCode { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressNumber { get; set; }
    public string? AddressComplement { get; set; }
    public string? AddressNeighborhood { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressState { get; set; }
    public string OriginType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? NotesSummary { get; set; }
    public bool AcceptsMarketing { get; set; }
    public DateTime? LgpdConsentAt { get; set; }
    public DateTime? FirstServiceAt { get; set; }
    public DateTime? LastServiceAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public int Rn { get; set; }
    public int TotalCount { get; set; }
}