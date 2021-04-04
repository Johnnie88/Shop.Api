using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<List<Category>>> Get()
    {
        return new List<Category>();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        return new Category();
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post([FromBody] Category model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        return Ok(model);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model)
    {
        //Verifica se o ID informado é o mesmo do modelo
        if (model.Id != id)
            return NotFound(new { message = "Categoria não encontrada" });

        //Verifica se os dados são válidos
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(model);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Delete()
    {
        return Ok();
    }
}