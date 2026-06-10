using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FitnessTracker.Helpers;
using FitnessTracker.Models;

namespace FitnessTracker.Repositories
{
    public class ZielRepository
    {
        public async Task<IEnumerable<Ziel>> GetAllAsync()
        {
            using var db = DatabaseHelper.GetConnection();
            return await db.QueryAsync<Ziel>("SELECT * FROM Ziel");
        }

        public async Task AddAsync(Ziel ziel)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "INSERT INTO Ziel (Beschreibung, Zielgewicht, TaeglicheKalorien, Startdatum, Enddatum) " +
                "VALUES (@Beschreibung, @Zielgewicht, @TaeglicheKalorien, @Startdatum, @Enddatum)", ziel);
        }

        public async Task DeleteAsync(int id)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "DELETE FROM Ziel WHERE Id = @Id", new { Id = id });
        }
    }
}