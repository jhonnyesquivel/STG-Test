using STG.Core.Entities;
using STG.Core.Repository;
using STG.Core.Services;

namespace STG.Application
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;

        public AnimalService(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }

        public async Task CreateAnimal(Animal animal)
        {
            await _animalRepository.Create(animal);
        }

        public async Task UpdateAnimal(Animal animal)
        {
            await _animalRepository.Update(animal);
        }

        public async Task DeleteAnimal(int animalId)
        {
            await _animalRepository.Delete(animalId);
        }

        public async Task<List<Animal>> GetAnimalsOlderThanTwoYearsAndFemaleSortedByName()
        {
            return await _animalRepository.GetAnimalsOlderThanTwoYearsAndFemaleSortedByName();
        }

        public async Task<Dictionary<string, int>> GetAnimalCountsBySex()
        {
            return await _animalRepository.GetAnimalCountsBySex();
        }

        public async Task<List<Animal>> FilterAnimals(string? animalId, string? name, string? sex, string? status)
        {
            return await _animalRepository.FilterAnimals(animalId, name, sex, status);
        }
    }
}
