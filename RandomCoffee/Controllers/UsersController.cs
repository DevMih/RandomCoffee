using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomCoffee.Data;
using RandomCoffee.DTOs;
using RandomCoffee.Entities;
using RandomCoffee.Interfaces;
using RandomCoffee.Services;
using System.Security.Claims;

namespace RandomCoffee.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly SmtpService _smptService;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public UsersController(DataContext context, SmtpService smptService,
            IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            _smptService = smptService;
            _mapper = mapper;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            return Ok(await _context.Users
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .ToListAsync());
        }

        [HttpGet("{username}", Name = "GetUser")]
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            return await _context.Users
                .Where(u => u.UserName == username)
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<AppUser>> PostUser(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult<AppUser>> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Photo)
                .SingleOrDefaultAsync();

            _mapper.Map(memberUpdateDTO, user);

            _context.Users.Update(user);

            if (await _context.SaveChangesAsync() > 0) return NoContent();

            return BadRequest("Failed to update user");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var user = _context.Users
                .Where(u => u.UserName  == username)
                .FirstOrDefault();

            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Photo)
                .SingleOrDefaultAsync();

            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photo != null)
            {
                await DeletePhoto();
            }

            user.Photo = photo;

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtRoute("GetUser", new { username = user.UserName }, _mapper.Map<PhotoDTO>(photo));
            }
            return BadRequest("Problem adding photo");
        }

        [HttpDelete("delete-photo")]
        public async Task<ActionResult> DeletePhoto()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _context.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Photo)
                .SingleOrDefaultAsync();

            var photo = user.Photo;

            if (photo == null) return NotFound();

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photo = null;

            if (await _context.SaveChangesAsync() > 0) return Ok();

            return BadRequest("Failed to delete a photo");
        }

        [HttpPost("message")]
        public async Task<ActionResult<MessageDTO>> SendMessage(MessageDTO messageDTO)
        {
            await _smptService.SendEmail(messageDTO.To, messageDTO.Subject, messageDTO.Body);

            return Ok(messageDTO);
        }
    }
}
