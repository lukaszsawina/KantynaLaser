using AutoMapper;
using KantynaLaser.Web.Helper;
using KantynaLaser.Web.Interfaces;
using KantynaLaser.Web.Models;
using KantynaLaser.Web.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KantynaLaser.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUserProvider _userProvider;

        public UsersController(
            IMapper mapper, 
            ILogger<UsersController> logger, 
            IUserAccountRepository userAccountRepository, 
            IUserProvider userProvider)
        {
            _mapper = mapper;
            _logger = logger;
            _userAccountRepository = userAccountRepository;
            _userProvider = userProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                _logger.LogInformation("Attempting to receive all users from database");
                var users = await _userAccountRepository.GetUsers();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _logger.LogInformation("All users were send");
                return Ok(_mapper.Map<List<UserAccountDto>>(users));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while reveiving data from database");
                throw new Exception("Error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            _logger.LogInformation($"Attempting to receive user {id} from database");
            var user = await _userAccountRepository.GetUser(id);

            if (user is null)
            {
                _logger.LogWarning($"User with id {id} don't exist");
                return NotFound($"User with id {id} don't exist");
            }

            _logger.LogInformation("All users were send");
            return Ok(_mapper.Map<UserAccountDto>(user));
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            _logger.LogInformation($"Attempting to receive current user from database");
            var currentUserId = _userProvider.GetUserId();
            var user = await _userAccountRepository.GetUser(Guid.Parse(currentUserId));

            if (user is null)
            {
                _logger.LogWarning($"User with id {currentUserId} don't exist");
                return NotFound($"User with id {currentUserId} don't exist");
            }

            _logger.LogInformation("All users were send");
            return Ok(_mapper.Map<UserAccountDto>(user));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserAccountDto userToUpdate)
        {
            _logger.LogInformation($"Attempting to update user with id {userToUpdate.Id}");

            var user = await _userAccountRepository.GetUser(userToUpdate.Id);

            if(user is null)
            {
                _logger.LogWarning($"User with id {userToUpdate.Id} don't exist");
                return NotFound($"User with id {userToUpdate.Id} don't exist");
            }

            try
            {
                user.Update(userToUpdate.FirstName, userToUpdate.LastName);
                await _userAccountRepository.UpdateUser(user);

                return Ok();
            }
            catch(ArgumentException ex) 
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    }
}

