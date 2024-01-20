using KantynaLaser.Web.Helper;
using KantynaLaser.Web.Interfaces;
using KantynaLaser.Web.Models;
using KantynaLaser.Web.Models.DTO;
using KantynaLaser.Web.Repository;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KantynaLaser.Web.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class IdentityController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<IdentityController> _logger;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserIdentityRepository _userIdentityRepository;
    private readonly IUserAccountRepository _userAccountRepository;

    public IdentityController(
        IConfiguration configuration, 
        ILogger<IdentityController> logger, 
        IPasswordHasher passwordHasher,
        IUserIdentityRepository userIdentity, 
        IUserAccountRepository userAccount)
    {
        _configuration = configuration;
        _logger = logger;
        _passwordHasher = passwordHasher;
        _userIdentityRepository = userIdentity;
        _userAccountRepository = userAccount;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto register)
    {
        _logger.LogInformation($"Attempting to add user with email {register.Email}");

        if (await _userAccountRepository.GetUserByEmail(register.Email) is not null)
        {
            _logger.LogInformation($"User with email {register.Email} already exist");
            return Conflict($"User with email {register.Email} already exist");
        }

        try
        {
            var userIdentity = new UserIdentity(
                register.FirstName,
                register.LastName, 
                register.Email, 
                _passwordHasher.HashPassword(register.Password));

            userIdentity.User.UserIdentity = userIdentity;

            await _userIdentityRepository.CreateIdentity(userIdentity);

            return Ok("Successfully Registered");
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex.Message);
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto login)
    {
        var userIdentity = await _userIdentityRepository.GetIdentityByEmail(login.Email);

        if(userIdentity is null)
        {
            _logger.LogInformation($"User with email {login.Email} don't exist");
            return NotFound($"UIser with email {login.Email} don't exist");
        }

        var result = _passwordHasher.VerifyPassword(userIdentity.Password, login.Password);

        if(!result)
        {
            _logger.LogWarning($"Invalid email and password");
            return BadRequest($"Invalid email and password");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var user = await _userAccountRepository.GetUserByEmail(login.Email);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var Sectoken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

        return Ok(token);
    }
}
