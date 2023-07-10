using STG.Core.Entities;

namespace STG.Core.Services
{
    public interface IAnimalService
    {
        Task CreateAnimal(Animal animal);
        Task UpdateAnimal(Animal animal);
        Task DeleteAnimal(int animalId);
        Task<List<Animal>> GetAnimalsOlderThanTwoYearsAndFemaleSortedByName();
        Task<Dictionary<string, int>> GetAnimalCountsBySex();
        Task<List<Animal>> FilterAnimals(string? animalId, string? name, string? sex, string? status);
    }
}
