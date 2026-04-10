namespace ClubeBeneficios.Customers.Domain.Dtos.Filters;

public sealed class PartnerCustomerAdminFilterDto
{
    public string? Search { get; set; }
    public Guid? PartnerId { get; set; }
    public string? Status { get; set; }
    public string? RegistrationStage { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}