namespace ClubeBeneficios.Customers.Domain.Dtos.Common;

public sealed class PartnerOptionDto
{
    public Guid Id { get; set; }
    public string TradeName { get; set; } = string.Empty;
    public string? Status { get; set; }
    public string? Segment { get; set; }
    public string? Category { get; set; }
}