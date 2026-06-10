using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using FitnessTracker.ViewModels;

namespace FitnessTracker.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainWindowViewModel();
            DataContext = vm;

            // Обновляем график когда меняются точки
            vm.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(vm.GewichtChartPoints))
                    DrawGewichtChart(vm.GewichtChartPoints);
            };
        }

        private void DrawGewichtChart(string pointsStr)
        {
            var canvas = this.FindControl<Canvas>("GewichtCanvas");
            if (canvas == null) return;

            canvas.Children.Clear();

            if (string.IsNullOrWhiteSpace(pointsStr)) return;

            var parts = pointsStr.Split(' ');
            var points = parts
                .Select(p => p.Split(','))
                .Where(xy => xy.Length == 2)
                .Select(xy => new Avalonia.Point(
                    double.Parse(xy[0], System.Globalization.CultureInfo.InvariantCulture),
                    double.Parse(xy[1], System.Globalization.CultureInfo.InvariantCulture)))
                .ToList();

            if (points.Count < 2) return;

            // Рисуем линии между точками
            for (int i = 0; i < points.Count - 1; i++)
            {
                var line = new Line
                {
                    StartPoint = points[i],
                    EndPoint = points[i + 1],
                    Stroke = new SolidColorBrush(Color.Parse("#34D399")),
                    StrokeThickness = 2.5,
                    StrokeLineCap = PenLineCap.Round
                };
                canvas.Children.Add(line);
            }

            // Рисуем точки
            foreach (var pt in points)
            {
                var dot = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = new SolidColorBrush(Color.Parse("#34D399")),
                };
                Canvas.SetLeft(dot, pt.X - 4);
                Canvas.SetTop(dot, pt.Y - 4);
                canvas.Children.Add(dot);
            }
        }
    }
}