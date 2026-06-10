using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FitnessTracker.Helpers;
using FitnessTracker.Models;

namespace FitnessTracker.Repositories
{
    public class GewichtRepository
    {
        public async Task<IEnumerable<Gewicht>> GetAllAsync()
        {
            using var db = DatabaseHelper.GetConnection();
            return await db.QueryAsync<Gewicht>(
                "SELECT * FROM Gewicht ORDER BY Datum DESC");
        }

        public async Task AddAsync(Gewicht gewicht)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "INSERT INTO Gewicht (Datum, Wert) VALUES (@Datum, @Wert)", gewicht);
        }

        public async Task DeleteAsync(int id)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "DELETE FROM Gewicht WHERE Id = @Id", new { Id = id });
        }
    }
}