using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext dataContext)
        {
            var employee = new User { Id = 1, UserName = "robin", Password = "robin", Role = "employee"};
            var manager = new User { Id = 1, UserName = "batman", Password = "batman", Role = "manager"};
            var category = new Category { Id = 1, Title = "Informática"};
            var product = new Product { Id = 1, Category = category, Title = "Mouse", Price = 299, Description = "Mouse Gamer"};

            dataContext.Users.Add(employee);
            dataContext.Users.Add(manager);
            dataContext.Categories.Add(category);
            dataContext.Products.Add(product);
            await dataContext.SaveChangesAsync();
            
            return Ok(
                new { message = "Dados configurados"} 
            );

        }
    }
}