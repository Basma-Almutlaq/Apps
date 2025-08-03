using Microsoft.EntityFrameworkCore;
using CleanArch.Application.Abstractions;
using CleanArch.Domain.Entities;
using CleanArch.Infrastructure.Data;

namespace CleanArch.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public Task<Product?> GetByIdAsync(int id, CancellationToken ct = default)
        => _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<List<Product>> GetAllAsync(CancellationToken ct = default)
        => _db.Products.AsNoTracking().ToListAsync(ct);

    public async Task<Product> AddAsync(Product product, CancellationToken ct = default)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);
        return product;
    }

    public async Task UpdateAsync(Product product, CancellationToken ct = default)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.Products.FindAsync(new object?[] { id }, ct);
        if (entity is null) return;
        _db.Products.Remove(entity);
        await _db.SaveChangesAsync(ct);
    }
}