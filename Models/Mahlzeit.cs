using System;

namespace FitnessTracker.Models
{
    public class Mahlzeit
    {
        public int Id { get; set; }
        public string Typ { get; set; } = "Snack";
        public DateTime Datum { get; set; } = DateTime.Today;
        public string Name { get; set; } = "";
        public int Kalorien { get; set; }
        public decimal Protein { get; set; }
        public decimal Kohlenhydrate { get; set; }
        public decimal Fett { get; set; }
    }
}