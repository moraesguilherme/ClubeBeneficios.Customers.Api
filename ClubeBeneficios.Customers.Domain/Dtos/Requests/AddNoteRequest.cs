namespace ClubeBeneficios.Customers.Domain.Dtos.Requests;

public sealed class AddNoteRequest
{
    public string NoteType { get; set; } = "general";
    public string? Title { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsInternal { get; set; } = true;
}