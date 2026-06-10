namespace FitnessTracker.Models
{
    public class Profil
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Alter_jahre { get; set; }
        public int Groesse { get; set; }
        public decimal Gewicht { get; set; }
        public string Ziel { get; set; } = "Gewicht halten";
        public int TaeglicheKalorien { get; set; }
    }
}