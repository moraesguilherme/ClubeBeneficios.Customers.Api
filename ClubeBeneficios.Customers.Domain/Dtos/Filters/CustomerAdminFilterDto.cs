namespace ClubeBeneficios.Customers.Domain.Dtos.Filters;

public sealed class CustomerAdminFilterDto
{
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? OriginType { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}