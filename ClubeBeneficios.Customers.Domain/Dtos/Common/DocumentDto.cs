namespace ClubeBeneficios.Customers.Domain.Dtos.Common;

public sealed class DocumentDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public Guid? PetId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string? FileName { get; set; }
    public string? MimeType { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? ExpiresAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public Guid? VerifiedByUserId { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}