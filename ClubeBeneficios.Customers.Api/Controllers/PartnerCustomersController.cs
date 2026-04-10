using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubeBeneficios.Customers.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/admin/partner-customers")]
public class PartnerCustomersController : ControllerBase
{
    private readonly IPartnerCustomerService _partnerCustomerService;

    public PartnerCustomersController(IPartnerCustomerService partnerCustomerService)
    {
        _partnerCustomerService = partnerCustomerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPagedAsync(
        [FromQuery] PartnerCustomerFilterDto filter,
        CancellationToken cancellationToken)
    {
        var result = await _partnerCustomerService.GetPagedAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _partnerCustomerService.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreatePartnerCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _partnerCustomerService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        [FromBody] UpdatePartnerCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await _partnerCustomerService.UpdateAsync(id, request, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}