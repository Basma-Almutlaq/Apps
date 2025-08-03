using CleanArch.Domain.Entities;

namespace CleanArch.Application.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(CancellationToken ct = default);
    Task<Product?> GetAsync(int id, CancellationToken ct = default);
    Task<Product> CreateAsync(Product p, CancellationToken ct = default);
    Task UpdateAsync(Product p, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}