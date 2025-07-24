using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public ProductsController(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,User")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _ctx.Products.ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            _ctx.Products.Add(product);
            await _ctx.SaveChangesAsync();
            return Ok(product);
        }
    }
}