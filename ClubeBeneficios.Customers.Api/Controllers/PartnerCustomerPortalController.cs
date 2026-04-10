using ClubeBeneficios.Customers.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubeBeneficios.Customers.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/partner-customer/me")]
public class PartnerCustomerPortalController : ControllerBase
{
    private readonly IPartnerCustomerPortalService _service;

    public PartnerCustomerPortalController(IPartnerCustomerPortalService service)
    {
        _service = service;
    }

    [HttpGet("benefits/dashboard-summary")]
    public async Task<IActionResult> GetBenefitDashboardAsync(CancellationToken cancellationToken)
    {
        var result = await _service.GetMyBenefitDashboardAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("benefits/requests")]
    public async Task<IActionResult> GetBenefitRequestsAsync(CancellationToken cancellationToken)
    {
        var result = await _service.GetMyBenefitRequestsAsync(cancellationToken);
        return Ok(result);
    }
}