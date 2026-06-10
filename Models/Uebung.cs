namespace FitnessTracker.Models
{
    public class Uebung
    {
        public int Id { get; set; }
        public int WorkoutId { get; set; }
        public string Name { get; set; } = "";
        public decimal Gewicht { get; set; }
        public int Wiederholungen { get; set; }
        public int Saetze { get; set; }
    }
}