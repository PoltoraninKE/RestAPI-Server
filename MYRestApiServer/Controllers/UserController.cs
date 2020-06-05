using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYRestApiServer.Core.Models;

namespace MYRestApiServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly Core.Models.AppContext ctx;

        public UserController(MYRestApiServer.Core.Models.AppContext _ctx)
        {
            ctx = _ctx;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await ctx.Users.ToListAsync(HttpContext.RequestAborted));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var answer = await ctx.Users.Include(det => det.Details).FirstOrDefaultAsync(usr => usr.ID == id);
            if (answer == null)
            {
                return NotFound();
            }
            return Ok(answer);
        }

        [HttpGet("{login}")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            var answer = await ctx.Users.Include(det => det.Details).FirstOrDefaultAsync(usr => usr.Login == login);
            if (answer == null)
            {
                return NotFound();
            }
            return Ok(answer);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User User)
        {
            ctx.Users.Update(User);
            await ctx.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserByID(int id) 
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.ID == id);
            ctx.Remove(user);
            await ctx.SaveChangesAsync();
            return Ok();
        }
        /* Login might be not identity
        [HttpDelete("{login:string}")]
        public async Task<IActionResult> DeleteUserByLogin(string _login) 
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.Login == _login);
            ctx.Remove(user);
            await ctx.SaveChangesAsync();
            return Ok();
        }*/
    }
}