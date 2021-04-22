using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

//Endpoint = URL
//https://localhost:5001/categories/
//http://localhost:5000
//https://meuapp.azurewebsites.bet/

[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().ToListAsync(); //AsNoTracking tras menos informações no objeto
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id); //AsNoTracking tras menos informações no objeto
        return Ok(categories);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles= "employee")]
    public async Task<ActionResult<Category>> Post([FromBody] Category model,
                                                   [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            context.Categories.Add(model);
            await context.SaveChangesAsync();
        }
        catch
        {
            BadRequest(new { message = "Não foi possivel criar a categoria" });
        }

        return Ok(model);
    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles= "employee")]
    public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model,
                                                   [FromServices] DataContext context)
    {
        //Verifica se o ID informado é o mesmo do modelo
        if (model.Id != id)
            return NotFound(new { message = "Categoria não encontrada" });

        try
        {
            //Verifica se os dados são válidos
            if (!ModelState.IsValid)
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();


            }
        }
        catch (DbUpdateConcurrencyException)
        {
            BadRequest(new { message = "Este registro já foi atualizado" });
        }
        catch (Exception)
        {
            BadRequest(new { message = "Não foi possivel atualizar a sua categoria" });
        }

        return Ok(model);
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles= "employee")]
    public async Task<ActionResult<Category>> Delete(int id,
                                                   [FromServices] DataContext context)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
        {
            return NotFound(new { message = "Categoria não encontrada" });
        }

        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();

        }
        catch (Exception)
        {

            BadRequest(new { message = "Não foi possivel remover a sua categoria" });
        }

        return Ok(category);
    }
}