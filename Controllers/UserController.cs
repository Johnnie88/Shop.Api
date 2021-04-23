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
    [Route("v1/users")]
    public class UserController : Controller
    {
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post([FromBody] User model,
                                                   [FromServices] DataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //força o usuário a ser sempre "funcionário"
                model.Role = "employee";
                context.Users.Add(model);
                await context.SaveChangesAsync();
            }
            catch
            {
                BadRequest(new { message = "Não foi possivel criar a categoria" });
            }

            //Esconde a senha
            model.Password = "";

            return model;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
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

            //Esconde a senha
            model.Password = "";

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