using ClubeBeneficios.Customers.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubeBeneficios.Customers.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/customer/me")]
public class CustomerPortalController : ControllerBase
{
    private readonly ICustomerPortalService _service;

    public CustomerPortalController(ICustomerPortalService service)
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