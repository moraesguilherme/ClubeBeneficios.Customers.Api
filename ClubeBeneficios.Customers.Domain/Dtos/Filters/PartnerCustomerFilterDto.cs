namespace ClubeBeneficios.Customers.Domain.Dtos.Filters;

public sealed class PartnerCustomerFilterDto
{
    public Guid? PartnerId { get; set; }
    public string? Search { get; set; }
    public string? Status { get; set; }
    public string? RegistrationStage { get; set; }
    public string? OriginType { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}