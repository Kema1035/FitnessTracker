using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FitnessTracker.Helpers;
using FitnessTracker.Models;

namespace FitnessTracker.Repositories
{
    public class WorkoutRepository
    {
        public async Task<IEnumerable<Workout>> GetAllAsync()
        {
            using var db = DatabaseHelper.GetConnection();
            return await db.QueryAsync<Workout>(
                "SELECT * FROM Workout ORDER BY Datum DESC");
        }

        public async Task AddAsync(Workout workout)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "INSERT INTO Workout (Datum, Typ, Dauer, Kalorien, Notizen) " +
                "VALUES (@Datum, @Typ, @Dauer, @Kalorien, @Notizen)", workout);
        }

        public async Task DeleteAsync(int id)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "DELETE FROM Workout WHERE Id = @Id", new { Id = id });
        }

        public async Task UpdateAsync(Workout workout)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "UPDATE Workout SET Datum=@Datum, Typ=@Typ, Dauer=@Dauer, " +
                "Kalorien=@Kalorien, Notizen=@Notizen WHERE Id=@Id", workout);
        }
    }
}