using CleanArch.Application.Services;
using CleanArch.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _svc;
    public ProductsController(IProductService svc) => _svc = svc;

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAll(CancellationToken ct)
        => await _svc.GetAllAsync(ct);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product?>> Get(int id, CancellationToken ct)
    {
        var p = await _svc.GetAsync(id, ct);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product p, CancellationToken ct)
    {
        var created = await _svc.CreateAsync(p, ct);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Product p, CancellationToken ct)
    {
        if (id != p.Id) return BadRequest("ID mismatch.");
        await _svc.UpdateAsync(p, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await _svc.DeleteAsync(id, ct);
        return NoContent();
    }
}