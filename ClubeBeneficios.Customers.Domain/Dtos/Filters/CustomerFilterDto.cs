namespace ClubeBeneficios.Customers.Domain.Dtos.Filters;

public sealed class CustomerFilterDto
{
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? Level { get; set; }
    public string? Segment { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}