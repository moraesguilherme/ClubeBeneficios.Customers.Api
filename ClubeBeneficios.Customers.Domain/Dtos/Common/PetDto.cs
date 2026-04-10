namespace ClubeBeneficios.Customers.Domain.Dtos.Common;

public sealed class PetDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string? Breed { get; set; }
    public string? Sex { get; set; }
    public DateTime? BirthDate { get; set; }
    public int? AgeMonths { get; set; }
    public decimal? WeightKg { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
    public bool IsNeutered { get; set; }
    public DateTime? NeuteredAt { get; set; }
    public string BehaviorStatus { get; set; } = string.Empty;
    public string? TemperamentSummary { get; set; }
    public string? RestrictionNotes { get; set; }
    public string? MedicalNotes { get; set; }
    public string? FeedingNotes { get; set; }
    public string? SpecialCareNotes { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}