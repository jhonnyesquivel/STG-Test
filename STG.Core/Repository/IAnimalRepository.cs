using STG.Core.Entities;

namespace STG.Core.Repository
{
    public interface IAnimalRepository
    {
        Task<Animal> GetById(int animalId);
        Task Create(Animal animal);
        Task Update(Animal animal);
        Task Delete(int animalId);
        Task<List<Animal>> GetAnimalsOlderThanTwoYearsAndFemaleSortedByName();
        Task<Dictionary<string, int>> GetAnimalCountsBySex();
        Task<List<Animal>> FilterAnimals(int? animalId = null, string? name = null, string? sex = null, string? status = null);
    }
}
