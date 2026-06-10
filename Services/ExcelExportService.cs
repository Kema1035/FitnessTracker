using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FitnessTracker.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FitnessTracker.Services
{
    public class ExcelExportService
    {
        // Устанавливаем лицензию один раз статически для всего класса
        static ExcelExportService()
        {
            ExcelPackage.License.SetNonCommercialPersonal("FitnessTracker");
        }

        public async Task ExportWorkoutsAsync(IEnumerable<Workout> workouts, string filePath)
        {
            await Task.Run(() =>
            {
                using var package = new ExcelPackage();
                var sheet = package.Workbook.Worksheets.Add("Workouts");

                sheet.Cells[1, 1].Value = "Datum";
                sheet.Cells[1, 2].Value = "Typ";
                sheet.Cells[1, 3].Value = "Dauer (Min)";
                sheet.Cells[1, 4].Value = "Kalorien";
                sheet.Cells[1, 5].Value = "Notizen";

                using (var range = sheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(70, 130, 180));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                int row = 2;
                foreach (var w in workouts)
                {
                    sheet.Cells[row, 1].Value = w.Datum.ToString("dd.MM.yyyy");
                    sheet.Cells[row, 2].Value = w.Typ;
                    sheet.Cells[row, 3].Value = w.Dauer;
                    sheet.Cells[row, 4].Value = w.Kalorien;
                    sheet.Cells[row, 5].Value = w.Notizen;
                    row++;
                }

                sheet.Cells.AutoFitColumns();
                package.SaveAs(new FileInfo(filePath));
            });
        }

        public async Task ExportMahlzeitenAsync(IEnumerable<Mahlzeit> mahlzeiten, string filePath)
        {
            await Task.Run(() =>
            {
                using var package = new ExcelPackage();
                var sheet = package.Workbook.Worksheets.Add("Mahlzeiten");

                sheet.Cells[1, 1].Value = "Datum";
                sheet.Cells[1, 2].Value = "Name";
                sheet.Cells[1, 3].Value = "Kalorien";
                sheet.Cells[1, 4].Value = "Protein (g)";
                sheet.Cells[1, 5].Value = "Kohlenhydrate (g)";
                sheet.Cells[1, 6].Value = "Fett (g)";

                using (var range = sheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(60, 179, 113));
                    range.Style.Font.Color.SetColor(System.Drawing.Color.White);
                }

                int row = 2;
                foreach (var m in mahlzeiten)
                {
                    sheet.Cells[row, 1].Value = m.Datum.ToString("dd.MM.yyyy");
                    sheet.Cells[row, 2].Value = m.Name;
                    sheet.Cells[row, 3].Value = m.Kalorien;
                    sheet.Cells[row, 4].Value = m.Protein;
                    sheet.Cells[row, 5].Value = m.Kohlenhydrate;
                    sheet.Cells[row, 6].Value = m.Fett;
                    row++;
                }

                sheet.Cells.AutoFitColumns();
                package.SaveAs(new FileInfo(filePath));
            });
        }
    }
}