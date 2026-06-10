using System;

namespace FitnessTracker.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; } = DateTime.Today;
        public string Typ { get; set; } = "";
        public int Dauer { get; set; }
        public int Kalorien { get; set; }
        public string Notizen { get; set; } = "";
    }
}