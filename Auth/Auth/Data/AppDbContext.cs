using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
public class AppDbContext : IdentityDbContext<IdentityUser>
{
public AppDbContext(DbContextOptions options) : base(options) {}
public DbSet<Product> Products { get; set;}
}