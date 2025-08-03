using CleanArch.Application.Abstractions;
using CleanArch.Domain.Entities;

namespace CleanArch.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    public ProductService(IProductRepository repo) => _repo = repo;

    public Task<List<Product>> GetAllAsync(CancellationToken ct = default) => _repo.GetAllAsync(ct);
    public Task<Product?> GetAsync(int id, CancellationToken ct = default) => _repo.GetByIdAsync(id, ct);

    public async Task<Product> CreateAsync(Product p, CancellationToken ct = default)
    {
        if (p.Price < 0) throw new ArgumentException("Price must be non-negative.");
        return await _repo.AddAsync(p, ct);
    }

    public async Task UpdateAsync(Product p, CancellationToken ct = default)
    {
        if (p.Price < 0) throw new ArgumentException("Price must be non-negative.");
        await _repo.UpdateAsync(p, ct);
    }

    public Task DeleteAsync(int id, CancellationToken ct = default) => _repo.DeleteAsync(id, ct);
}