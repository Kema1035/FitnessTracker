using System.Data;
using MySql.Data.MySqlClient;

namespace FitnessTracker.Helpers
{
    public class DatabaseHelper
    {
        private static string _connectionString =
            "Server=localhost;Port=3306;Database=fitnesstracker;Uid=root;Pwd=root;";

        public static IDbConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}