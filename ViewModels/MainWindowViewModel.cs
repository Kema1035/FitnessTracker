using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessTracker.Models;
using FitnessTracker.Repositories;
using FitnessTracker.Services;


namespace FitnessTracker.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly WorkoutRepository _workoutRepo = new();
        private readonly MahlzeitRepository _mahlzeitRepo = new();
        private readonly GewichtRepository _gewichtRepo = new();
        private readonly ZielRepository _zielRepo = new();
        private readonly ProfilRepository _profilRepo = new();
        private readonly ExcelExportService _exportService = new();
        private readonly FoodSearchService _foodSearchService = new();
        private readonly UebungRepository _uebungRepo = new();

        [ObservableProperty] private ObservableCollection<Workout> workouts = new();
        private Workout? _selectedWorkout;
        public Workout? SelectedWorkout
        {
            get => _selectedWorkout;
            set
            {
                SetProperty(ref _selectedWorkout, value);
                _ = LoadUebungenAsync();
            }
        }
        [ObservableProperty] private string newWorkoutTyp = "";
        [ObservableProperty] private int newWorkoutDauer;
        [ObservableProperty] private int newWorkoutKalorien;
        [ObservableProperty] private string newWorkoutNotizen = "";

        [ObservableProperty] private ObservableCollection<Mahlzeit> mahlzeiten = new();
        [ObservableProperty] private Mahlzeit? selectedMahlzeit;
        [ObservableProperty] private string newMahlzeitName = "";
        [ObservableProperty] private int newMahlzeitKalorien;
        [ObservableProperty] private decimal newMahlzeitProtein;
        [ObservableProperty] private decimal newMahlzeitKohlenhydrate;
        [ObservableProperty] private decimal newMahlzeitFett;
        [ObservableProperty] private string newMahlzeitTyp = "Fruehstueck";

        [ObservableProperty] private ObservableCollection<Mahlzeit> fruehstueck = new();
        [ObservableProperty] private ObservableCollection<Mahlzeit> mittagessen = new();
        [ObservableProperty] private ObservableCollection<Mahlzeit> abendessen = new();
        [ObservableProperty] private ObservableCollection<Mahlzeit> snacks = new();
        [ObservableProperty] private string fruehstueckKalorien = "0";
        [ObservableProperty] private string mittagessenKalorien = "0";
        [ObservableProperty] private string abendessenKalorien = "0";
        [ObservableProperty] private string snacksKalorien = "0";

        [ObservableProperty] private ObservableCollection<Gewicht> gewichte = new();
        [ObservableProperty] private Gewicht? selectedGewicht;
        [ObservableProperty] private decimal newGewichtWert;

        // График — строка точек для Polyline
        [ObservableProperty] private string gewichtChartPoints = "";
        [ObservableProperty] private double gewichtMin = 0;
        [ObservableProperty] private double gewichtMax = 100;
        [ObservableProperty] private string kalorienRingColor = "#34D399";

        [ObservableProperty] private ObservableCollection<Ziel> ziele = new();
        [ObservableProperty] private string newZielBeschreibung = "";
        [ObservableProperty] private decimal newZielgewicht;
        [ObservableProperty] private int newZielKalorien;
        [ObservableProperty] private Ziel? selectedZiel;

        [ObservableProperty] private string profilName = "";
        [ObservableProperty] private int profilAlter;
        [ObservableProperty] private int profilGroesse;
        [ObservableProperty] private decimal profilGewicht;
        [ObservableProperty] private string profilZiel = "Gewicht halten";
        [ObservableProperty] private int profilKalorien = 2500;

        [ObservableProperty] private string gesamtKalorien = "0";
        [ObservableProperty] private string gesamtWorkouts = "0";
        [ObservableProperty] private string aktuellesGewicht = "-";
        [ObservableProperty] private string statusMessage = "Willkommen beim Fitness Tracker!";
        [ObservableProperty] private string kalorienVerbraucht = "0";
        [ObservableProperty] private string kalorienZiel = "2500";
        [ObservableProperty] private string kalorienRestlich = "2500";
        [ObservableProperty] private string workoutKalorienVerbrannt = "0";

        [ObservableProperty] private AvaloniaList<double> kalorienDashArray = new() { 0, 502 };
        [ObservableProperty] private string kalorienZielText = "0 / 2500 kcal";
        [ObservableProperty] private decimal gesamtProtein;
        [ObservableProperty] private decimal gesamtKohlenhydrate;
        [ObservableProperty] private decimal gesamtFett;
        [ObservableProperty] private double proteinBarWidth = 0;
        [ObservableProperty] private double kohlenhydrateBarWidth = 0;
        [ObservableProperty] private double fettBarWidth = 0;
        [ObservableProperty] private string heuteKalorien = "0";
        [ObservableProperty] private string heuteProtein = "0";
        [ObservableProperty] private string heuteKohlenhydrate = "0";
        [ObservableProperty] private string heuteFett = "0";
        [ObservableProperty] private string heuteWorkouts = "0";
        [ObservableProperty] private string heuteWorkoutDauer = "0";
        [ObservableProperty] private string heuteWorkoutKalorien = "0";
        [ObservableProperty] private string kalorienArcData = "";
        // Uebungen
        [ObservableProperty] private ObservableCollection<Uebung> uebungen = new();
        [ObservableProperty] private Uebung? selectedUebung;
        [ObservableProperty] private string newUebungName = "";
        [ObservableProperty] private decimal newUebungGewicht;
        [ObservableProperty] private int newUebungWiederholungen;
        [ObservableProperty] private int newUebungSaetze;
        // AI
        [ObservableProperty] private string aiAntwort = "";
        [ObservableProperty] private bool aiLaeuft = false;

        private string _foodSearchQuery = "";
        public string FoodSearchQuery
        {
            get => _foodSearchQuery;
            set
            {
                SetProperty(ref _foodSearchQuery, value);
                _ = SearchFoodDebounced();
            }
        }
        private CancellationTokenSource? _searchCts;

        [ObservableProperty] private ObservableCollection<FoodProduct> foodSearchResults = new();
        [ObservableProperty] private FoodProduct? selectedFoodProduct;
        [ObservableProperty] private int foodMenge = 100;
        [ObservableProperty] private string newFoodTyp = "Frühstück";
        public string[] MahlzeitTypen { get; } = { "Frühstück", "Mittagessen", "Abendessen", "Snack" };

        public MainWindowViewModel()
        {
            _ = LoadAllDataAsync();
        }

        private async Task LoadAllDataAsync()
        {
            await LoadWorkoutsAsync();
            await LoadMahlzeitenAsync();
            await LoadGewichtAsync();
            await LoadZieleAsync();
            await LoadProfilAsync();
            UpdateStatistik();
            UpdateGewichtChart();
        }

        private void UpdateStatistik()
        {
            var totalKcal = Mahlzeiten.Sum(m => m.Kalorien);
            GesamtKalorien = totalKcal.ToString();
            GesamtWorkouts = Workouts.Count.ToString();
            WorkoutKalorienVerbrannt = Workouts.Sum(w => w.Kalorien).ToString();
            AktuellesGewicht = Gewichte.Any()
                ? Gewichte.OrderByDescending(g => g.Datum).First().Wert + " kg"
                : ProfilGewicht > 0 ? ProfilGewicht + " kg" : "-";

            GesamtProtein = Mahlzeiten.Sum(m => m.Protein);
            GesamtKohlenhydrate = Mahlzeiten.Sum(m => m.Kohlenhydrate);
            GesamtFett = Mahlzeiten.Sum(m => m.Fett);

            var ziel = ProfilKalorien > 0 ? ProfilKalorien : 2500;
            KalorienVerbraucht = totalKcal.ToString();
            KalorienZiel = ziel.ToString();
            KalorienRestlich = Math.Max(0, ziel - totalKcal).ToString();

            var progress = Math.Min((double)totalKcal / ziel, 1.0);
            var circumference = 2 * Math.PI * 73;
            Console.WriteLine($"Progress: {progress}, circumference: {circumference}, filled: {progress * circumference}, empty: {(1 - progress) * circumference}");
            KalorienDashArray = new AvaloniaList<double> { (1 - progress) * circumference, progress * circumference };
            KalorienRingColor = totalKcal > ziel ? "#F87171" : "#34D399";
            // Arc для кружка
            var angle = progress * 360.0;
            var rad = angle * Math.PI / 180.0;
            var cx = 80.0; var cy = 80.0; var r = 73.0;
            var x = cx + r * Math.Sin(rad);
            var y = cy - r * Math.Cos(rad);
            var largeArc = angle > 180 ? 1 : 0;
            // Цвет кольца — зелёный если норма, красный если превышено
            var ringColor = totalKcal > ziel ? "#F87171" : "#34D399";
            KalorienArcData = progress <= 0
                ? ""
                : progress >= 1
                    ? $"M {cx},{cy-r} A {r},{r} 0 1 1 {cx-0.01},{cy-r}"
                    : string.Format(System.Globalization.CultureInfo.InvariantCulture,
                        "M {0},{1} A {2},{2} 0 {3} 1 {4},{5}",
                        cx, cy - r, r, largeArc, x, y);
            KalorienZielText = $"{totalKcal} / {ziel} kcal";

            const double maxWidth = 200.0;
            const double maxGrams = 200.0;
            ProteinBarWidth = Math.Min((double)GesamtProtein / maxGrams * maxWidth, maxWidth);
            KohlenhydrateBarWidth = Math.Min((double)GesamtKohlenhydrate / maxGrams * maxWidth, maxWidth);
            FettBarWidth = Math.Min((double)GesamtFett / maxGrams * maxWidth, maxWidth);

            Fruehstueck = new ObservableCollection<Mahlzeit>(Mahlzeiten.Where(m => m.Typ == "Frühstück"));
            Mittagessen = new ObservableCollection<Mahlzeit>(Mahlzeiten.Where(m => m.Typ == "Mittagessen"));
            Abendessen = new ObservableCollection<Mahlzeit>(Mahlzeiten.Where(m => m.Typ == "Abendessen"));
            Snacks = new ObservableCollection<Mahlzeit>(Mahlzeiten.Where(m => m.Typ == "Snack" || string.IsNullOrEmpty(m.Typ)));

            FruehstueckKalorien = Fruehstueck.Sum(m => m.Kalorien).ToString();
            MittagessenKalorien = Mittagessen.Sum(m => m.Kalorien).ToString();
            AbendessenKalorien = Abendessen.Sum(m => m.Kalorien).ToString();
            SnacksKalorien = Snacks.Sum(m => m.Kalorien).ToString();
            // Сводка дня
            var heute = DateTime.Today;
            var heuteMahlzeiten = Mahlzeiten.Where(m => m.Datum.Date == heute).ToList();
            var heuteWorkoutList = Workouts.Where(w => w.Datum.Date == heute).ToList();

            HeuteKalorien = heuteMahlzeiten.Sum(m => m.Kalorien).ToString();
            HeuteProtein = heuteMahlzeiten.Sum(m => m.Protein).ToString("F1");
            HeuteKohlenhydrate = heuteMahlzeiten.Sum(m => m.Kohlenhydrate).ToString("F1");
            HeuteFett = heuteMahlzeiten.Sum(m => m.Fett).ToString("F1");
            HeuteWorkouts = heuteWorkoutList.Count.ToString();
            HeuteWorkoutDauer = heuteWorkoutList.Sum(w => w.Dauer).ToString();
            HeuteWorkoutKalorien = heuteWorkoutList.Sum(w => w.Kalorien).ToString();
        }

        private void UpdateGewichtChart()
        {
            if (!Gewichte.Any())
            {
                GewichtChartPoints = "";
                return;
            }

            var sorted = Gewichte.OrderBy(g => g.Datum).TakeLast(10).ToList();

            if (sorted.Count == 1)
            {
                GewichtMin = (double)sorted[0].Wert - 1;
                GewichtMax = (double)sorted[0].Wert + 1;
                GewichtChartPoints = "440,65";
                return;
            }

            var minW = (double)sorted.Min(g => g.Wert) - 0.5;
            var maxW = (double)sorted.Max(g => g.Wert) + 0.5;
            GewichtMin = Math.Round(minW, 1);
            GewichtMax = Math.Round(maxW, 1);

            const double chartWidth = 880;
            const double chartHeight = 110;

            var sb = new StringBuilder();
            for (int i = 0; i < sorted.Count; i++)
            {
                var x = i * (chartWidth / (sorted.Count - 1));
                var y = chartHeight - ((double)sorted[i].Wert - minW) / (maxW - minW) * chartHeight;
                if (i > 0) sb.Append(' ');
                sb.Append(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F1},{1:F1}", x, y));
            }
            GewichtChartPoints = sb.ToString();
        }

        private async Task SearchFoodDebounced()
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;
            try
            {
                await Task.Delay(500, token);
                if (string.IsNullOrWhiteSpace(_foodSearchQuery))
                {
                    FoodSearchResults = new ObservableCollection<FoodProduct>();
                    return;
                }
                StatusMessage = "Suche laeuft...";
                var results = await _foodSearchService.SearchAsync(_foodSearchQuery);
                if (!token.IsCancellationRequested)
                {
                    FoodSearchResults = new ObservableCollection<FoodProduct>(results);
                    StatusMessage = results.Count > 0
                        ? $"{results.Count} Produkte gefunden"
                        : "Keine Produkte gefunden";
                }
            }
            catch (TaskCanceledException) { }
        }

        [RelayCommand]
        private async Task LoadWorkoutsAsync()
        {
            var data = await _workoutRepo.GetAllAsync();
            Workouts = new ObservableCollection<Workout>(data);
        }

        [RelayCommand]
        private async Task AddWorkoutAsync()
        {
            if (string.IsNullOrWhiteSpace(NewWorkoutTyp)) return;
            await _workoutRepo.AddAsync(new Workout
            {
                Datum = DateTime.Today,
                Typ = NewWorkoutTyp,
                Dauer = NewWorkoutDauer,
                Kalorien = NewWorkoutKalorien,
                Notizen = NewWorkoutNotizen
            });
            NewWorkoutTyp = ""; NewWorkoutDauer = 0; NewWorkoutKalorien = 0; NewWorkoutNotizen = "";
            await LoadWorkoutsAsync();
            UpdateStatistik();
            StatusMessage = "Workout gespeichert!";
        }

        [RelayCommand]
        private async Task DeleteWorkoutAsync()
        {
            if (SelectedWorkout == null) return;
            await _workoutRepo.DeleteAsync(SelectedWorkout.Id);
            await LoadWorkoutsAsync();
            UpdateStatistik();
            StatusMessage = "Workout geloescht!";
        }
        
        [RelayCommand]
        private async Task LoadUebungenAsync()
        {
            if (SelectedWorkout == null)
            {
                Uebungen = new ObservableCollection<Uebung>();
                return;
            }
            var data = await _uebungRepo.GetByWorkoutIdAsync(SelectedWorkout.Id);
            Uebungen = new ObservableCollection<Uebung>(data);
        }

        [RelayCommand]
        private async Task AddUebungAsync()
        
        {
            Console.WriteLine($"AddUebung called! SelectedWorkout: {SelectedWorkout?.Id}, Name: '{NewUebungName}'");
            if (SelectedWorkout == null || string.IsNullOrWhiteSpace(NewUebungName)) return;
            await _uebungRepo.AddAsync(new Uebung
            {
                
                WorkoutId = SelectedWorkout.Id,
                Name = NewUebungName,
                Gewicht = NewUebungGewicht,
                Wiederholungen = NewUebungWiederholungen,
                Saetze = NewUebungSaetze
            });
            NewUebungName = ""; NewUebungGewicht = 0; NewUebungWiederholungen = 0; NewUebungSaetze = 0;
            await LoadUebungenAsync();
            StatusMessage = "Uebung gespeichert!";
            
        }

        [RelayCommand]
        private async Task DeleteUebungAsync()
        {
            if (SelectedUebung == null) return;
            await _uebungRepo.DeleteAsync(SelectedUebung.Id);
            await LoadUebungenAsync();
            StatusMessage = "Uebung geloescht!";
        }

        [RelayCommand]
        private async Task LoadMahlzeitenAsync()
        {
            var data = await _mahlzeitRepo.GetAllAsync();
            Mahlzeiten = new ObservableCollection<Mahlzeit>(data);
        }

        [RelayCommand]
        private async Task AddMahlzeitAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMahlzeitName)) return;
            Console.WriteLine($"Typ beim Hinzufuegen: '{NewMahlzeitTyp}'");
            await _mahlzeitRepo.AddAsync(new Mahlzeit
            {
                Datum = DateTime.Today,
                Name = NewMahlzeitName,
                Kalorien = NewMahlzeitKalorien,
                Protein = NewMahlzeitProtein,
                Kohlenhydrate = NewMahlzeitKohlenhydrate,
                Fett = NewMahlzeitFett,
                Typ = NewMahlzeitTyp
            });
            NewMahlzeitName = ""; NewMahlzeitKalorien = 0; NewMahlzeitProtein = 0;
            NewMahlzeitKohlenhydrate = 0; NewMahlzeitFett = 0;
            await LoadMahlzeitenAsync();
            UpdateStatistik();
            StatusMessage = "Mahlzeit gespeichert!";
        }

        [RelayCommand]
        private async Task DeleteMahlzeitAsync()
        {
            if (SelectedMahlzeit == null) return;
            await _mahlzeitRepo.DeleteAsync(SelectedMahlzeit.Id);
            await LoadMahlzeitenAsync();
            UpdateStatistik();
            StatusMessage = "Mahlzeit geloescht!";
        }

        [RelayCommand]
        private async Task LoadGewichtAsync()
        {
            var data = await _gewichtRepo.GetAllAsync();
            Gewichte = new ObservableCollection<Gewicht>(data);
        }

        [RelayCommand]
        private async Task AddGewichtAsync()
        {
            if (NewGewichtWert <= 0) return;
            await _gewichtRepo.AddAsync(new Gewicht { Datum = DateTime.Today, Wert = NewGewichtWert });
            NewGewichtWert = 0;
            await LoadGewichtAsync();
            UpdateStatistik();
            UpdateGewichtChart();
            Console.WriteLine($"Chart points: '{GewichtChartPoints}'");
            Console.WriteLine($"Gewichte count: {Gewichte.Count}");
            StatusMessage = "Gewicht gespeichert!";
        }

        [RelayCommand]
        private async Task DeleteGewichtAsync()
        {
            if (SelectedGewicht == null) return;
            await _gewichtRepo.DeleteAsync(SelectedGewicht.Id);
            await LoadGewichtAsync();
            UpdateStatistik();
            UpdateGewichtChart();
            StatusMessage = "Eintrag geloescht!";
        }

        [RelayCommand]
        private async Task LoadZieleAsync()
        {
            var data = await _zielRepo.GetAllAsync();
            Ziele = new ObservableCollection<Ziel>(data);
        }

        [RelayCommand]
        private async Task DeleteSelectedZielAsync()
        {
            if (SelectedZiel == null) return;
            await _zielRepo.DeleteAsync(SelectedZiel.Id);
            await LoadZieleAsync();
            StatusMessage = "Ziel geloescht!";
        }

        [RelayCommand]
        private async Task AddZielAsync()
        {
            if (string.IsNullOrWhiteSpace(NewZielBeschreibung)) return;
            await _zielRepo.AddAsync(new Ziel
            {
                Beschreibung = NewZielBeschreibung,
                Zielgewicht = NewZielgewicht,
                TaeglicheKalorien = NewZielKalorien
            });
            NewZielBeschreibung = ""; NewZielgewicht = 0; NewZielKalorien = 0;
            await LoadZieleAsync();
            StatusMessage = "Ziel gespeichert!";
        }

        [RelayCommand]
        private async Task LoadProfilAsync()
        {
            var profil = await _profilRepo.GetAsync();
            if (profil != null)
            {
                ProfilName = profil.Name;
                ProfilAlter = profil.Alter_jahre;
                ProfilGroesse = profil.Groesse;
                ProfilGewicht = profil.Gewicht;
                ProfilZiel = profil.Ziel;
                ProfilKalorien = profil.TaeglicheKalorien > 0 ? profil.TaeglicheKalorien : 2500;
            }
        }

        [RelayCommand]
        private async Task SaveProfilAsync()
        {
            await _profilRepo.SaveAsync(new Profil
            {
                Name = ProfilName,
                Alter_jahre = ProfilAlter,
                Groesse = ProfilGroesse,
                Gewicht = ProfilGewicht,
                Ziel = ProfilZiel,
                TaeglicheKalorien = ProfilKalorien
            });
            UpdateStatistik();
            StatusMessage = "Profil gespeichert!";
        }

        [RelayCommand]
        private async Task AddFoodProductAsync(FoodProduct? product)
        {
            if (product == null) return;
            var faktor = FoodMenge / 100.0m;
            await _mahlzeitRepo.AddAsync(new Mahlzeit
            {
                Datum = DateTime.Today,
                Name = product.Name,
                Kalorien = (int)(product.Kalorien * faktor),
                Protein = product.Protein * faktor,
                Kohlenhydrate = product.Kohlenhydrate * faktor,
                Fett = product.Fett * faktor,
                Typ = NewFoodTyp
            });
            await LoadMahlzeitenAsync();
            UpdateStatistik();
            StatusMessage = $"{product.Name} hinzugefuegt!";
        }

        [RelayCommand]
        private async Task ExportWorkoutsAsync()
        {
            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"Workouts_{DateTime.Today:yyyyMMdd}.xlsx");
            await _exportService.ExportWorkoutsAsync(Workouts, path);
            StatusMessage = $"Exportiert: {path}";
        }

        [RelayCommand]
        private async Task ExportMahlzeitenAsync()
        {
            var path = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"Mahlzeiten_{DateTime.Today:yyyyMMdd}.xlsx");
            await _exportService.ExportMahlzeitenAsync(Mahlzeiten, path);
            StatusMessage = $"Exportiert: {path}";
        }
        
        [RelayCommand]
        private async Task GetAiAnalyseAsync()
        {
            AiLaeuft = true;
            AiAntwort = "Analyse läuft...";

            try
            {
                var prompt = $@"Du bist ein persönlicher Fitness- und Ernährungscoach. Analysiere folgende Daten und gib 3-4 konkrete motivierende Empfehlungen auf Deutsch:

WORKOUTS (letzte {Workouts.Count}):
{string.Join("\n", Workouts.Take(5).Select(w => $"- {w.Datum:dd.MM.yyyy}: {w.Typ}, {w.Dauer} Min, {w.Kalorien} kcal"))}

ERNAEHRUNG (letzte Mahlzeiten):
{string.Join("\n", Mahlzeiten.Take(5).Select(m => $"- {m.Name}, {m.Kalorien} kcal"))}

GEWICHT: {AktuellesGewicht}
TAGESZIEL: {ProfilKalorien} kcal
HEUTE GEGESSEN: {HeuteKalorien} kcal";

                using var client = new System.Net.Http.HttpClient();
                client.Timeout = TimeSpan.FromSeconds(60);

                var requestBody = System.Text.Json.JsonSerializer.Serialize(new
                {
                    model = "llama3.2:1b",
                    prompt = prompt,
                    stream = false
                });

                var response = await client.PostAsync(
                    "http://localhost:11434/api/generate",
                    new System.Net.Http.StringContent(requestBody, System.Text.Encoding.UTF8, "application/json"));

                var json = await response.Content.ReadAsStringAsync();
                var doc = System.Text.Json.JsonDocument.Parse(json);
                AiAntwort = doc.RootElement.GetProperty("response").GetString() ?? "Keine Antwort";
            }
            catch (Exception ex)
            {
                AiAntwort = $"Fehler: {ex.Message}";
            }
            finally
            {
                AiLaeuft = false;
            }
        }
    }
}