using CarnetSante.Core.Models;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;

namespace CarnetSante.WPF.Services;

public class PdfService : IPdfService
{
    private static readonly string ExportFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "CarnetSante", "Exports");

    public async Task<string> ExportCarnetCompletAsync(Patient patient)
    {
        return await Task.Run(() =>
        {
            Directory.CreateDirectory(ExportFolder);
            var fileName = $"Carnet_{patient.NumeroCarnet}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            var filePath = Path.Combine(ExportFolder, fileName);

            using var document = new PdfDocument();
            document.Info.Title = $"Carnet de Santé - {patient.NomComplet}";
            document.Info.Author = "CarnetSante - Ministère de la Santé";

            AddCoverPage(document, patient);
            AddConstantesPage(document, patient);
            AddOperationsPage(document, patient);
            AddVaccinationsPage(document, patient);
            AddVisitesPage(document, patient);

            document.Save(filePath);
            return filePath;
        });
    }

    public async Task<string> ExportConstantesAsync(Patient patient)
    {
        return await Task.Run(() =>
        {
            Directory.CreateDirectory(ExportFolder);
            var fileName = $"Constantes_{patient.NumeroCarnet}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            var filePath = Path.Combine(ExportFolder, fileName);

            using var document = new PdfDocument();
            AddConstantesPage(document, patient);
            document.Save(filePath);
            return filePath;
        });
    }

    private static void AddCoverPage(PdfDocument doc, Patient patient)
    {
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var w = page.Width;

        // Header background
        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(27, 94, 32)), 0, 0, w, 120);

        // Title
        var titleFont = new XFont("Arial", 22, XFontStyle.Bold);
        var subFont = new XFont("Arial", 14, XFontStyle.Regular);
        var bodyFont = new XFont("Arial", 11, XFontStyle.Regular);
        var boldFont = new XFont("Arial", 12, XFontStyle.Bold);

        gfx.DrawString("CARNET DE SANTÉ", titleFont, XBrushes.White,
            new XRect(0, 30, w, 40), XStringFormats.TopCenter);
        gfx.DrawString("Ministère de la Santé - Burkina Faso", subFont, XBrushes.White,
            new XRect(0, 70, w, 30), XStringFormats.TopCenter);

        // Patient info
        double y = 150;
        gfx.DrawString($"N° Carnet : {patient.NumeroCarnet}", boldFont, XBrushes.Black, 60, y);
        y += 25;
        gfx.DrawString($"Matricule : {patient.Matricule}", bodyFont, XBrushes.Black, 60, y);
        y += 25;
        gfx.DrawString($"Nom : {patient.NomComplet}", boldFont, XBrushes.Black, 60, y);
        y += 25;
        gfx.DrawString($"Date de naissance : {patient.DateNaissance:dd/MM/yyyy}", bodyFont, XBrushes.Black, 60, y);
        y += 25;
        gfx.DrawString($"Lieu de naissance : {patient.LieuNaissance}", bodyFont, XBrushes.Black, 60, y);
        y += 25;
        gfx.DrawString($"Sexe : {patient.Sexe}", bodyFont, XBrushes.Black, 60, y);
        y += 25;
        gfx.DrawString($"Groupe sanguin : {patient.GroupeSanguin}", bodyFont, XBrushes.Black, 60, y);

        // Footer
        gfx.DrawString($"Imprimé le {DateTime.Now:dd/MM/yyyy à HH:mm}", bodyFont, XBrushes.Gray,
            new XRect(0, page.Height - 30, w, 20), XStringFormats.TopCenter);
    }

    private static void AddConstantesPage(PdfDocument doc, Patient patient)
    {
        if (!patient.Constantes.Any()) return;
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var w = page.Width;
        var headerFont = new XFont("Arial", 14, XFontStyle.Bold);
        var bodyFont = new XFont("Arial", 10, XFontStyle.Regular);

        gfx.DrawString("B - CONSTANTES VITALES", headerFont, XBrushes.Black, 40, 40);

        double y = 80;
        foreach (var c in patient.Constantes.OrderByDescending(x => x.DateMesure).Take(20))
        {
            gfx.DrawString(
                $"  {c.DateMesure:dd/MM/yy}  |  Poids: {c.Poids}kg  Taille: {c.Taille}cm  TA: {c.TensionArterielle}  FC: {c.FrequenceCardiaque}bpm  T°: {c.Temperature}°C",
                bodyFont, XBrushes.Black, 40, y);
            y += 20;
            if (y > page.Height - 60) break;
        }
    }

    private static void AddOperationsPage(PdfDocument doc, Patient patient)
    {
        if (!patient.OperationsMedicales.Any()) return;
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var headerFont = new XFont("Arial", 14, XFontStyle.Bold);
        var bodyFont = new XFont("Arial", 10, XFontStyle.Regular);

        gfx.DrawString("D - OPÉRATIONS MÉDICALES", headerFont, XBrushes.Black, 40, 40);
        double y = 80;
        foreach (var op in patient.OperationsMedicales.OrderByDescending(x => x.DateOperation))
        {
            gfx.DrawString($"  {op.DateOperation:dd/MM/yyyy}  |  {op.Intitule}  |  {op.TypeIntervention}  |  {op.Etablissement}",
                bodyFont, XBrushes.Black, 40, y);
            y += 20;
            if (y > page.Height - 60) break;
        }
    }

    private static void AddVaccinationsPage(PdfDocument doc, Patient patient)
    {
        if (!patient.Vaccinations.Any()) return;
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var headerFont = new XFont("Arial", 14, XFontStyle.Bold);
        var bodyFont = new XFont("Arial", 10, XFontStyle.Regular);

        gfx.DrawString("E - VACCINATIONS", headerFont, XBrushes.Black, 40, 40);
        double y = 80;
        foreach (var v in patient.Vaccinations.OrderByDescending(x => x.DateVaccination))
        {
            gfx.DrawString($"  {v.DateVaccination:dd/MM/yyyy}  |  {v.NomVaccin}  |  Lot: {v.Lot}  |  {v.Centre}",
                bodyFont, XBrushes.Black, 40, y);
            y += 20;
            if (y > page.Height - 60) break;
        }
    }

    private static void AddVisitesPage(PdfDocument doc, Patient patient)
    {
        if (!patient.VisitesSanitaires.Any()) return;
        var page = doc.AddPage();
        var gfx = XGraphics.FromPdfPage(page);
        var headerFont = new XFont("Arial", 14, XFontStyle.Bold);
        var bodyFont = new XFont("Arial", 10, XFontStyle.Regular);

        gfx.DrawString("F - VISITES SANITAIRES", headerFont, XBrushes.Black, 40, 40);
        double y = 80;
        foreach (var v in patient.VisitesSanitaires.OrderByDescending(x => x.DateVisite))
        {
            gfx.DrawString($"  {v.DateVisite:dd/MM/yyyy}  |  {v.Motif}  |  {v.Etablissement}  |  Dr {v.MedecinTraitant}",
                bodyFont, XBrushes.Black, 40, y);
            y += 20;
            if (y > page.Height - 60) break;
        }
    }
}
