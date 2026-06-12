using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessTracker.Models;
using FitnessTracker.Repositories;

namespace FitnessTracker.ViewModels
{
    public partial class WorkoutDetailViewModel : ObservableObject
    {
        private readonly UebungRepository _uebungRepo = new();
        private readonly Workout _workout;

        [ObservableProperty] private ObservableCollection<Uebung> uebungen = new();
        [ObservableProperty] private Uebung? selectedUebung;
        [ObservableProperty] private string newUebungName = "";
        [ObservableProperty] private decimal newUebungGewicht;
        [ObservableProperty] private int newUebungWiederholungen;
        [ObservableProperty] private int newUebungSaetze;
        [ObservableProperty] private string workoutTitel = "";

        public WorkoutDetailViewModel(Workout workout)
        {
            _workout = workout;
            WorkoutTitel = $"{workout.Typ} — {workout.Datum:dd.MM.yyyy}";
            _ = LoadUebungenAsync();
        }

        private async Task LoadUebungenAsync()
        {
            var data = await _uebungRepo.GetByWorkoutIdAsync(_workout.Id);
            Uebungen = new ObservableCollection<Uebung>(data);
        }

        [RelayCommand]
        private async Task AddUebungAsync()
        {
            if (string.IsNullOrWhiteSpace(NewUebungName)) return;
            await _uebungRepo.AddAsync(new Uebung
            {
                WorkoutId = _workout.Id,
                Name = NewUebungName,
                Gewicht = NewUebungGewicht,
                Wiederholungen = NewUebungWiederholungen,
                Saetze = NewUebungSaetze
            });
            NewUebungName = ""; NewUebungGewicht = 0;
            NewUebungWiederholungen = 0; NewUebungSaetze = 0;
            await LoadUebungenAsync();
        }

        [RelayCommand]
        private async Task DeleteUebungAsync()
        {
            if (SelectedUebung == null) return;
            await _uebungRepo.DeleteAsync(SelectedUebung.Id);
            await LoadUebungenAsync();
        }
    }
}