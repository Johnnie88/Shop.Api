using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;

namespace ShopControllers
{
    [Route("users")]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<User>> Post([FromBody] User model,
                                                   [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
            }
            catch
            {
                BadRequest(new { message = "Não foi possivel criar a categoria" });
            }

            return Ok(model);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model,
                                                  [FromServices] DataContext context)
        {
            var user = await context.Users
                             .AsNoTracking()
                             .Where(x => x.UserName == model.UserName && x.Password == model.Password)
                             .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { message = "Usuário ou senha inválidos" });
            }

            var token = TokenService.GenerateToken(user);
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("anonimo")]
        [AllowAnonymous]
        public string Anonimo() => "Anonimo";

        [HttpGet]
        [Route("autenticado")]
        [Authorize]
        public string Autenticado() => "Autenticado";

        [HttpGet]
        [Route("funcionario")]
        [Authorize(Roles = "employee")]
        public string Funcionario() => "Funcionario";

        [HttpGet]
        [Route("gerente")]
        [Authorize(Roles = "manager")]
        public string Gerente() => "Gerente";


    }
}