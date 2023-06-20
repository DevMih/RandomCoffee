using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomCoffee.Data;
using RandomCoffee.Entities;
using RandomCoffee.Services;

namespace RandomCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly SmtpService _smptService;
        private readonly TimerService _timerService;

        public UsersController(DataContext context, SmtpService smptService, TimerService timerService)
        {
            _context = context;
            _smptService = smptService;
            _timerService = timerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            return user;
        }
        
        [HttpPost]
        public async Task<ActionResult<AppUser>> PostUser(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<AppUser>> PutUser(int id, AppUser newUser)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }

            user.UserName = newUser.UserName;
            user.FirstName = newUser.FirstName;
            user.SecondName = newUser.SecondName;
            user.Email = newUser.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
