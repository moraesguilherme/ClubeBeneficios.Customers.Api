namespace ClubeBeneficios.Customers.Domain.Dtos.Common;

public sealed class FilterOptionDto
{
    public string Bucket { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}