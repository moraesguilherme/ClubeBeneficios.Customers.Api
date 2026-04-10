namespace ClubeBeneficios.Customers.Domain.Dtos.Common;

public sealed class StatusHistoryDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string? OldStatus { get; set; }
    public string NewStatus { get; set; } = string.Empty;
    public string? OldRegistrationStage { get; set; }
    public string? NewRegistrationStage { get; set; }
    public string? Reason { get; set; }
    public DateTime ChangedAt { get; set; }
    public Guid? ChangedByUserId { get; set; }
}