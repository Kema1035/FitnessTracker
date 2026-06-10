using System;

namespace FitnessTracker.Models
{
    public class Gewicht
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; } = DateTime.Today;
        public decimal Wert { get; set; }
    }
}