using AutoMapper;
using KantynaLaser.Web.Helper;
using KantynaLaser.Web.Interfaces;
using KantynaLaser.Web.Models;
using KantynaLaser.Web.Models.DTO;
using KantynaLaser.Web.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace KantynaLaser.Web.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RecipesController : Controller
    {
        private readonly ILogger<RecipesController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserProvider _userProvider;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IUserAccountRepository _userAccountRepository;

        public RecipesController(
            ILogger<RecipesController> logger, 
            IMapper mapper, 
            IUserProvider userProvider, 
            IRecipeRepository recipeRepository, 
            IUserAccountRepository userAccountRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userProvider = userProvider;
            _recipeRepository = recipeRepository;
            _userAccountRepository = userAccountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecipes() 
        {
            _logger.LogInformation("Attempting to receive all recipes from database");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("All recipes were send");
            var result = await _recipeRepository.GetRecipes();

            return Ok(_mapper.Map<List<RecipeDto>>(result));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe([FromRoute] Guid id)
        {
            _logger.LogInformation($"Attempting to receive recipe with id {id} from database");

            var recipe = await _recipeRepository.GetRecipe(id);

            if(recipe is null)
            {
                _logger.LogWarning($"Recipe with id {id} don't exist");
                return NotFound($"Recipe with id {id} don't exist");
            }

            return Ok(_mapper.Map<RecipeDto>(recipe));
        }

        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserRecipes([FromRoute] Guid id)
        {
            _logger.LogInformation($"Attempting to receive recipes from user with id {id} from database");

            var user = await _userAccountRepository.GetUser(id);

            if (user is null)
            {
                _logger.LogWarning($"User with id {id} don't exist");
                return NotFound($"User with id {id} don't exist");
            }

            var recipes = await _recipeRepository.GetUserRecipes(user.Id);

            return Ok(_mapper.Map<List<RecipeDto>>(recipes));
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetPublicRecipes()
        {
            _logger.LogInformation("Attempting to receive all public recipes from database");

            var recipes = await _recipeRepository.GetPublicRecipes();

            return Ok(_mapper.Map<List<RecipeDto>>(recipes));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody] RecipeDto recipe)
        {
            _logger.LogInformation("Attempting to add recipe");

            var userId = _userProvider.GetUserId();

            var user = await _userAccountRepository.GetUser(Guid.Parse(userId));

            try
            {
                var recipeToCreate = new Recipe(
                    recipe.Title, 
                    recipe.Ingredients,
                    recipe.Steps,
                    recipe.Tools,
                    recipe.PreparationTime, 
                    recipe.CookingTime, 
                    recipe.IsPublic);

                user.Recipes.Add(recipeToCreate);

                await _recipeRepository.CreateRecipe(recipeToCreate);

                return Created();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateRecipe([FromBody] RecipeDto recipe)
        {
            _logger.LogInformation($"Attempting to update recipe with id {recipe.Id}");

            var userId = _userProvider.GetUserId();

            var user = await _userAccountRepository.GetUser(Guid.Parse(userId));

            var recipeToUpdate = await _recipeRepository.GetRecipe(recipe.Id);

            if(recipeToUpdate is null)
            {
                _logger.LogWarning($"Recipe with id {recipe.Id} don't exist");
                return NotFound($"Recipe with id {recipe.Id} don't exist");
            }

            if(recipeToUpdate.UserAccountId != user.Id)
            {
                _logger.LogWarning($"User with id {user.Id} cannot update recipe with id {recipeToUpdate.Id}");
                return BadRequest($"User with id {user.Id} cannot update recipe with id {recipeToUpdate.Id}");
            }

            try
            {
                recipeToUpdate.Update(
                    recipe.Title,
                    recipe.Ingredients,
                    recipe.Steps,
                    recipe.Tools,
                    recipe.PreparationTime,
                    recipe.CookingTime,
                    recipe.IsPublic);

                await _recipeRepository.UpdateRecpe(recipeToUpdate);

                return Ok("Successfully updated");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe([FromRoute] Guid id)
        {
            _logger.LogInformation($"Attempting to delete recipe with id {id}");

            var userId = _userProvider.GetUserId();

            var user = await _userAccountRepository.GetUser(Guid.Parse(userId));

            var recipeToDelete = await _recipeRepository.GetRecipe(id);

            if (recipeToDelete is null)
            {
                _logger.LogWarning($"Recipe with id {id} don't exist");
                return NotFound($"Recipe with id {id} don't exist");
            }

            if (recipeToDelete.UserAccountId != user.Id)
            {
                _logger.LogWarning($"User with id {user.Id} cannot delete recipe with id {recipeToDelete.Id}");
                return BadRequest($"User with id {user.Id} cannot delete recipe with id {recipeToDelete.Id}");
            }

            await _recipeRepository.DeleteRecipe(recipeToDelete);

            return Ok("Successfully deleted");
        }

    }
}
