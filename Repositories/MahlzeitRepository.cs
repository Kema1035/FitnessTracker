using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FitnessTracker.Helpers;
using FitnessTracker.Models;

namespace FitnessTracker.Repositories
{
    public class MahlzeitRepository
    {
        public async Task<IEnumerable<Mahlzeit>> GetAllAsync()
        {
            using var db = DatabaseHelper.GetConnection();
            return await db.QueryAsync<Mahlzeit>(
                "SELECT * FROM Mahlzeit ORDER BY Datum DESC");
        }

        public async Task AddAsync(Mahlzeit mahlzeit)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "INSERT INTO Mahlzeit (Datum, Name, Kalorien, Protein, Kohlenhydrate, Fett, Typ) " +
                "VALUES (@Datum, @Name, @Kalorien, @Protein, @Kohlenhydrate, @Fett, @Typ)", mahlzeit);
        }

        public async Task DeleteAsync(int id)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "DELETE FROM Mahlzeit WHERE Id = @Id", new { Id = id });
        }
    }
}