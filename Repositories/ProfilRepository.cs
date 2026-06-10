using System.Threading.Tasks;
using Dapper;
using FitnessTracker.Helpers;
using FitnessTracker.Models;

namespace FitnessTracker.Repositories
{
    public class ProfilRepository
    {
        public async Task<Profil?> GetAsync()
        {
            using var db = DatabaseHelper.GetConnection();
            return await db.QueryFirstOrDefaultAsync<Profil>(
                "SELECT * FROM Profil LIMIT 1");
        }

        public async Task SaveAsync(Profil profil)
        {
            using var db = DatabaseHelper.GetConnection();
            var exists = await db.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(*) FROM Profil");
            if (exists == 0)
                await db.ExecuteAsync(
                    "INSERT INTO Profil (Name, Alter_Jahre, Groesse, Gewicht, Ziel, TaeglicheKalorien) " +
                    "VALUES (@Name, @Alter_Jahre, @Groesse, @Gewicht, @Ziel, @TaeglicheKalorien)", profil);
            else
                await db.ExecuteAsync(
                    "UPDATE Profil SET Name=@Name, Alter_Jahre=@Alter_Jahre, Groesse=@Groesse, " +
                    "Gewicht=@Gewicht, Ziel=@Ziel, TaeglicheKalorien=@TaeglicheKalorien", profil);
        }
    }
}