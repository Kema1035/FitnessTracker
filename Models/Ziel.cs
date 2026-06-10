using System;

namespace FitnessTracker.Models
{
    public class Ziel
    {
        public int Id { get; set; }
        public string Beschreibung { get; set; } = "";
        public decimal Zielgewicht { get; set; }
        public int TaeglicheKalorien { get; set; }
        public DateTime Startdatum { get; set; } = DateTime.Today;
        public DateTime Enddatum { get; set; } = DateTime.Today.AddMonths(3);
    }
}