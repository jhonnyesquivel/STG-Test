using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STG.Core.Entities;
using STG.Core.Services;

namespace STG.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] Animal animal)
        {
            await _animalService.CreateAnimal(animal);
            return Ok();
        }

        [HttpPut("{animalId}")]
        public async Task<IActionResult> UpdateAnimal(int animalId, [FromBody] Animal animal)
        {
            if (animalId != animal.AnimalId)
            {
                return BadRequest("The animal ID does not match the ID specified in the path.");
            }

            await _animalService.UpdateAnimal(animal);
            return Ok();
        }

        [HttpDelete("{animalId}")]
        public async Task<IActionResult> DeleteAnimal(int animalId)
        {
            await _animalService.DeleteAnimal(animalId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> FilterAnimals([FromQuery] string? animalId, [FromQuery] string? name, [FromQuery] string? sex, [FromQuery] string? status)
        {
            var animals = await _animalService.FilterAnimals(animalId, name, sex, status);
            return Ok(animals);
        }
    }
}