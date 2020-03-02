using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

// Endpoint => URL
// http://localhost:5000
// https://localhost:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get(
        [FromServices] DataContext dataContext
        )
    {
        var categories = await dataContext.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(
        int id,
        [FromServices] DataContext dataContext
        )
    {
        var category = await dataContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return Ok(category);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Post(
        [FromBody]Category model,
        [FromServices] DataContext dataContext
        )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            dataContext.Categories.Add(model);
            await dataContext.SaveChangesAsync();
            return Ok(model);
        }
        catch
        {
            return BadRequest(new { message = "Não foi possível criar a categoria." });
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<List<Category>>> Put
                    (int id,
                    [FromBody] Category model,
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
            dataContext.Entry<Category>(model).State = EntityState.Modified;
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
    public async Task<ActionResult<List<Category>>> Delete(
        int id,
        [FromServices] DataContext dataContext
    )
    {
        var category = await dataContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound(new { message = "Categoria não encontrada" });

        try
        {
            dataContext.Categories.Remove(category);
            await dataContext.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso." });

        }
        catch (Exception)
        {
            return BadRequest("Falha ao remover o registro no banco de dados.");
        }
    }
}