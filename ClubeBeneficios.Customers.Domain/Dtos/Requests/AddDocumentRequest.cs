namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class AddDocumentRequest
{
    public Guid? PetId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string? FileName { get; set; }
    public string? MimeType { get; set; }
    public string Status { get; set; } = "pending";
    public DateTime? ExpiresAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public Guid? VerifiedByUserId { get; set; }
    public string? RejectionReason { get; set; }
}