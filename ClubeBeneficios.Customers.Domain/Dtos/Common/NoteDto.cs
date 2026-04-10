namespace ClubeBeneficios.Customers.Domain.Dtos.Common;

public sealed class NoteDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string NoteType { get; set; } = string.Empty;
    public string? Title { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsInternal { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedByUserId { get; set; }
}