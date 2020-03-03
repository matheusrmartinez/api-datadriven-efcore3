using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                context.Users.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário." });
            }
        }

        // [HttpPost]
        // [Route("login")]
        // public async Task<ActionResult<dynamic>> Authenticate([FromServices] DataContext context, [FromBody] User model)
        // {
        //     var user = context.Users
        //        .AsNoTracking()
        //        .Where(x => x.UserName == model.UserName && x.Password == model.Password)
        //        .FirstOrDefaultAsync();

        //     if(user == null)
        //     return NotFound(new { message = "Usuário ou senha inválidos"});

        //     var token = TokenService.GenerateToken(user);
        //     return new


        //     catch (Exception)
        //     {
        //         return BadRequest(new { message = "Não foi possível criar o usuário." });
        //     }
        // }
    }
}