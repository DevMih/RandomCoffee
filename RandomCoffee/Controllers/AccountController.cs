using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomCoffee.Data;
using RandomCoffee.DTOs;
using RandomCoffee.Entities;
using RandomCoffee.Interfaces;
using RandomCoffee.Services;
using System.Security.Cryptography;
using System.Text;

namespace RandomCoffee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly SmtpService _smtpService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper, SmtpService smtpService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
            _smtpService = smtpService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.Username))
            {
                return BadRequest("Username is taken");
            }

            if (await _userManager.FindByEmailAsync(registerDTO.Email) != null)
            {
                return BadRequest("Email is taken");
            }

            var user = _mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.Username.ToLower();

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if(!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new {token, email = user.Email}, Request.Scheme);
            var subject = "Confirmation email link";
            _smtpService.SendEmail(user.Email, subject, confirmationLink);

            return new UserDTO
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                EmailConfirmed = false
            };
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return Redirect("http://localhost:4200/members");
                }
            }
            return BadRequest();
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.Users
                .Include(x => x.Photo)
                .SingleOrDefaultAsync(x => x.UserName == loginDTO.Username.ToLower());

            if (user == null)
            {
                return Unauthorized("Invalid username");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded) return Unauthorized();

            return new UserDTO
            {
                Username = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photo?.Url,
                EmailConfirmed = user.EmailConfirmed
            };
        }
    }
}
