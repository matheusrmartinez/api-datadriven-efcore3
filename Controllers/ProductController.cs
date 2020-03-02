using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

// Endpoint => URL
// https://localhost:5001/products
[Route("products")]
public class ProductController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Get(
        [FromServices] DataContext dataContext
        )
    {
        var products = await dataContext
        .Products
        .Include(x => x.Category)
        .AsNoTracking()
        .ToListAsync();
        return Ok(products);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Product>> GetById(
        int id,
        [FromServices] DataContext dataContext
        )
    {
        var Product = await dataContext
        .Products.Include(x => x.Category)
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == id);
        return Ok(Product);
    }


    [HttpGet] //products/categories/1
    [Route("categories/{id:int}")]
    public async Task<ActionResult<Product>> GetByCategory(
        int id,
        [FromServices] DataContext dataContext
        )
    {
        var Product = await dataContext
        .Products.Include(x => x.Category)
        .AsNoTracking()
        .Where(x => x.CategoryId == id)
        .ToListAsync();
        return Ok(Product);
    }


    [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Product>>> Post(
        [FromBody]Product model,
        [FromServices] DataContext dataContext
        )
    {
        if (ModelState.IsValid)
        {
            dataContext.Products.Add(model);
            await dataContext.SaveChangesAsync();
            return Ok(model);

        }
        else
        {
            return BadRequest(ModelState);
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Product>>> Put
                    (int id,
                    [FromBody] Product model,
                    [FromServices] DataContext dataContext
                    )
    {
        // Verifica se o ID informado é o mesmo do modelo
        if (id != model.Id)
            return NotFound(new { message = "Categoria não encontrada" });

        // Verifica se os dados são válidos
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            dataContext.Entry<Product>(model).State = EntityState.Modified;
            await dataContext.SaveChangesAsync();
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Este registro foi atualizado durante a modificação da categoria" });
        }

        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possível atualizar a categoria" });
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Product>>> Delete(
        int id,
        [FromServices] DataContext dataContext
    )
    {
        var Product = await dataContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (Product == null)
            return NotFound(new { message = "Categoria não encontrada" });

        try
        {
            dataContext.Products.Remove(Product);
            await dataContext.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso." });

        }
        catch (Exception)
        {
            return BadRequest("Falha ao remover o registro no banco de dados.");
        }
    }
}