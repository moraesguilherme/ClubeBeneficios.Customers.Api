using ClubeBeneficios.Customers.Domain.Dtos.Filters;
using ClubeBeneficios.Customers.Domain.Dtos.Requests;
using ClubeBeneficios.Customers.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClubeBeneficios.Customers.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/admin/customers")]
public class AdminCustomersController : ControllerBase
{
    private readonly ICustomerAdminService _service;

    public AdminCustomersController(ICustomerAdminService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> SearchPagedAsync([FromQuery] CustomerAdminFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _service.SearchPagedAsync(filter, cancellationToken);
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummaryAsync(CancellationToken cancellationToken)
    {
        var result = await _service.GetDashboardSummaryAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("filter-options")]
    public async Task<IActionResult> GetFilterOptionsAsync(CancellationToken cancellationToken)
    {
        var result = await _service.GetFilterOptionsAsync(cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerAdminRequest request, CancellationToken cancellationToken)
    {
        var id = await _service.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateCustomerAdminRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateAsync(id, request, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{id:guid}/pets")]
    public async Task<IActionResult> GetPetsAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetPetsAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/pets")]
    public async Task<IActionResult> AddPetAsync(Guid id, [FromBody] CreatePetRequest request, CancellationToken cancellationToken)
    {
        var petId = await _service.AddPetAsync(id, request, cancellationToken);
        return Ok(new { id = petId });
    }

    [HttpPut("{id:guid}/pets/{petId:guid}")]
    public async Task<IActionResult> UpdatePetAsync(Guid id, Guid petId, [FromBody] UpdatePetRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdatePetAsync(id, petId, request, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("{id:guid}/documents")]
    public async Task<IActionResult> GetDocumentsAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetDocumentsAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/documents")]
    public async Task<IActionResult> AddDocumentAsync(Guid id, [FromBody] AddDocumentRequest request, CancellationToken cancellationToken)
    {
        var documentId = await _service.AddDocumentAsync(id, request, cancellationToken);
        return Ok(new { id = documentId });
    }

    [HttpGet("{id:guid}/notes")]
    public async Task<IActionResult> GetNotesAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetNotesAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/notes")]
    public async Task<IActionResult> AddNoteAsync(Guid id, [FromBody] AddNoteRequest request, CancellationToken cancellationToken)
    {
        var noteId = await _service.AddNoteAsync(id, request, cancellationToken);
        return Ok(new { id = noteId });
    }

    [HttpGet("{id:guid}/status-history")]
    public async Task<IActionResult> GetStatusHistoryAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _service.GetStatusHistoryAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatusAsync(Guid id, [FromBody] UpdateClientStatusRequest request, CancellationToken cancellationToken)
    {
        var updated = await _service.UpdateStatusAsync(id, request, cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
}