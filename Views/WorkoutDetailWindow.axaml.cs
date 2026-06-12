using Avalonia.Controls;
using FitnessTracker.Models;
using FitnessTracker.ViewModels;

namespace FitnessTracker.Views
{
    public partial class WorkoutDetailWindow : Window
    {
        public WorkoutDetailWindow(Workout workout)
        {
            InitializeComponent();
            DataContext = new WorkoutDetailViewModel(workout);
        }
    }
}