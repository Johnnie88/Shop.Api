using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context)
        {
            var products = await context
                                .Products
                                .Include(x => x.Category) //join
                                .AsNoTracking()
                                .ToListAsync();

            return products;
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices] DataContext context)
        {
            var categories = await context
                                .Products
                                .Include(x => x.Category)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == id); //AsNoTracking tras menos informações no objeto

            return Ok(categories);
        }

        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id)
        {
            var products = await context
                                .Products
                                .Include(x => x.Category)
                                .AsNoTracking()
                                .Where(x => x.CategoryId == id)
                                .ToListAsync(); //AsNoTracking tras menos informações no objeto

            return Ok(products);
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<Product>> Post([FromBody] Product model,
                                                       [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
            }
            catch
            {
                BadRequest(new { message = "Não foi possivel criar o produto" });
            }

            return Ok(model);
        }


    }
}