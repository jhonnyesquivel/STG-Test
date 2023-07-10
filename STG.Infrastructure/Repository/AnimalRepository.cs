using Dapper;
using Microsoft.Extensions.Configuration;
using STG.Core.Entities;
using STG.Core.Repository;
using System.Data.SqlClient;

namespace STG.Infrastructure.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly string _connectionString;

        public AnimalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Animal> GetById(int animalId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var animal = await connection.QuerySingleOrDefaultAsync<Animal>("SELECT * FROM Animal WHERE AnimalId = @AnimalId", new { AnimalId = animalId });
            return animal;
        }

        public async Task Create(Animal animal)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var insertQuery = "INSERT INTO Animal (AnimalId, Name, Breed, BirthDate, Sex, Price, Status) VALUES (@AnimalId, @Name, @Breed, @BirthDate, @Sex, @Price, @Status); SELECT SCOPE_IDENTITY()";
            var animalId = await connection.ExecuteScalarAsync<int>(insertQuery, animal);
            animal.AnimalId = animalId;
        }

        public async Task Update(Animal animal)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var updateQuery = "UPDATE Animal SET Name = @Name, Breed = @Breed, BirthDate = @BirthDate, Sex = @Sex, Price = @Price, Status = @Status WHERE AnimalId = @AnimalId";
            await connection.ExecuteAsync(updateQuery, animal);
        }

        public async Task Delete(int animalId)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var deleteQuery = "DELETE FROM Animal WHERE AnimalId = @AnimalId";
            await connection.ExecuteAsync(deleteQuery, new { AnimalId = animalId });
        }

        public async Task<List<Animal>> GetAnimalsOlderThanTwoYearsAndFemaleSortedByName()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var query = "SELECT * FROM Animal WHERE BirthDate < DATEADD(YEAR, -2, GETDATE()) AND Sex = 'Female' ORDER BY Name";
            var animals = await connection.QueryAsync<Animal>(query);
            return animals.ToList();
        }

        public async Task<Dictionary<string, int>> GetAnimalCountsBySex()
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var query = "SELECT Sex, COUNT(*) AS Count FROM Animal GROUP BY Sex";
            var result = await connection.QueryAsync(query);
            return result.ToDictionary(row => (string)row.Sex, row => (int)row.Count);
        }

        public async Task<List<Animal>> FilterAnimals(string? animalId, string? name, string? sex, string? status)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();
            var query = "SELECT * FROM Animal WHERE (@AnimalId IS NULL OR AnimalId = @AnimalId) AND (@Name IS NULL OR Name = @Name) AND (@Sex IS NULL OR Sex = @Sex) AND (@Status IS NULL OR Status = @Status)";
            var animals = await connection.QueryAsync<Animal>(query, new { AnimalId = animalId, Name = name, Sex = sex, Status = status });
            return animals.ToList();
        }
    }
}