using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using FitnessTracker.Helpers;
using FitnessTracker.Models;

namespace FitnessTracker.Repositories
{
    public class UebungRepository
    {
        public async Task<IEnumerable<Uebung>> GetByWorkoutIdAsync(int workoutId)
        {
            using var db = DatabaseHelper.GetConnection();
            return await db.QueryAsync<Uebung>(
                "SELECT * FROM Uebung WHERE WorkoutId = @WorkoutId", new { WorkoutId = workoutId });
        }

        public async Task AddAsync(Uebung uebung)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "INSERT INTO Uebung (WorkoutId, Name, Gewicht, Wiederholungen, Saetze) " +
                "VALUES (@WorkoutId, @Name, @Gewicht, @Wiederholungen, @Saetze)", uebung);
        }

        public async Task DeleteAsync(int id)
        {
            using var db = DatabaseHelper.GetConnection();
            await db.ExecuteAsync(
                "DELETE FROM Uebung WHERE Id = @Id", new { Id = id });
        }
    }
}