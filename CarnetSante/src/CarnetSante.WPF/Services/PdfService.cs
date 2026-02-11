using CarnetSante.Core.Models;
using CarnetSante.Core.Services;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;

namespace CarnetSante.WPF.Services;

/// <summary>
/// Service de génération PDF du carnet de santé via PdfSharpCore.
/// </summary>
public class PdfService : IPdfService
{
    private const double MargeGauche = 40;
    private const double MargeHaut = 40;
    private const double LargeurPage = 595;  // A4
    private const double HauteurPage = 842;  // A4
    private const double LargeurContenu = LargeurPage - 2 * MargeGauche;

    public async Task<byte[]> GenererCarnetCompletAsync(Patient patient)
    {
        using var doc = new PdfDocument();
        doc.Info.Title = $"Carnet de Santé - {GetNomPatient(patient)}";
        doc.Info.Author = "CarnetSante v1.0";
        doc.Info.Subject = "Carnet Sanitaire Officiel";

        // Page de garde
        AjouterPageGarde(doc, patient);

        // Section A - État Civil
        if (patient.EtatCivil != null)
            AjouterSectionEtatCivil(doc, patient.EtatCivil);

        // Section B - Constantes
        if (patient.Constantes.Any())
            AjouterSectionConstantes(doc, patient.Constantes.ToList());

        // Section C - Examen d'Incorporation
        if (patient.ExamenIncorporation != null)
            AjouterSectionExamenIncorporation(doc, patient.ExamenIncorporation);

        // Section D - Opérations Médicales
        if (patient.OperationsMedicales.Any())
            AjouterSectionOperations(doc, patient.OperationsMedicales.ToList());

        // Section E - Vaccinations
        if (patient.Vaccinations.Any())
            AjouterSectionVaccinations(doc, patient.Vaccinations.ToList());

        // Section F - Visites Sanitaires
        if (patient.VisitesSanitaires.Any())
            AjouterSectionVisites(doc, patient.VisitesSanitaires.ToList());

        // Section G - Indisponibilités
        if (patient.Indisponibilites.Any())
            AjouterSectionIndisponibilites(doc, patient.Indisponibilites.ToList());

        // Section H - Certificats
        if (patient.CertificatsMedicaux.Any())
            AjouterSectionCertificats(doc, patient.CertificatsMedicaux.ToList());

        // Section I - Décisions de Réforme
        if (patient.DecisionsReforme.Any())
            AjouterSectionDecisionsReforme(doc, patient.DecisionsReforme.ToList());

        // Section J - Contrôle Fin de Service
        if (patient.ControleFinService != null)
            AjouterSectionControleFinService(doc, patient.ControleFinService);

        using var ms = new MemoryStream();
        doc.Save(ms);
        return await Task.FromResult(ms.ToArray());
    }

    public async Task<byte[]> GenererSectionAsync(Patient patient, string section)
    {
        using var doc = new PdfDocument();
        doc.Info.Title = $"Carnet - Section {section} - {GetNomPatient(patient)}";

        switch (section.ToUpper())
        {
            case "A": if (patient.EtatCivil != null) AjouterSectionEtatCivil(doc, patient.EtatCivil); break;
            case "B": AjouterSectionConstantes(doc, patient.Constantes.ToList()); break;
            case "C": if (patient.ExamenIncorporation != null) AjouterSectionExamenIncorporation(doc, patient.ExamenIncorporation); break;
            case "D": AjouterSectionOperations(doc, patient.OperationsMedicales.ToList()); break;
            case "E": AjouterSectionVaccinations(doc, patient.Vaccinations.ToList()); break;
            case "F": AjouterSectionVisites(doc, patient.VisitesSanitaires.ToList()); break;
            case "G": AjouterSectionIndisponibilites(doc, patient.Indisponibilites.ToList()); break;
            case "H": AjouterSectionCertificats(doc, patient.CertificatsMedicaux.ToList()); break;
            case "I": AjouterSectionDecisionsReforme(doc, patient.DecisionsReforme.ToList()); break;
            case "J": if (patient.ControleFinService != null) AjouterSectionControleFinService(doc, patient.ControleFinService); break;
        }

        using var ms = new MemoryStream();
        doc.Save(ms);
        return await Task.FromResult(ms.ToArray());
    }

    public async Task SauvegarderPdfAsync(byte[] pdfData, string cheminFichier)
    {
        await File.WriteAllBytesAsync(cheminFichier, pdfData);
    }

    public async Task OuvrirPdfAsync(byte[] pdfData, string nomFichierTemp)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), nomFichierTemp);
        await File.WriteAllBytesAsync(tempPath, pdfData);
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = tempPath,
            UseShellExecute = true
        });
    }

    // ── Méthodes privées de rendu ─────────────────────────────

    private void AjouterPageGarde(PdfDocument doc, Patient patient)
    {
        var page = doc.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);

        var fontTitre = new XFont("Arial", 22, XFontStyle.Bold);
        var fontSousTitre = new XFont("Arial", 14, XFontStyle.Regular);
        var fontNormal = new XFont("Arial", 12, XFontStyle.Regular);
        var bleu = XBrushes.DarkBlue;

        // En-tête institutionnel
        gfx.DrawString("BURKINA FASO", fontTitre, bleu,
            new XRect(0, 60, LargeurPage, 30), XStringFormats.TopCenter);
        gfx.DrawString("Unité - Progrès - Justice", fontSousTitre, XBrushes.Black,
            new XRect(0, 92, LargeurPage, 25), XStringFormats.TopCenter);

        // Ligne de séparation
        gfx.DrawLine(XPens.DarkBlue, MargeGauche, 125, LargeurPage - MargeGauche, 125);

        gfx.DrawString("CARNET DE SANTÉ SANITAIRE OFFICIEL", fontTitre, bleu,
            new XRect(0, 145, LargeurPage, 35), XStringFormats.TopCenter);

        // Infos patient
        double y = 230;
        if (patient.EtatCivil != null)
        {
            AjouterLigneInfo(gfx, fontNormal, "N° Carnet :", patient.NumeroCarnet, ref y);
            AjouterLigneInfo(gfx, fontNormal, "Nom :", patient.EtatCivil.Nom.ToUpper(), ref y);
            AjouterLigneInfo(gfx, fontNormal, "Prénoms :", patient.EtatCivil.Prenoms, ref y);
            AjouterLigneInfo(gfx, fontNormal, "Date de naissance :",
                patient.EtatCivil.DateNaissance.ToString("dd/MM/yyyy"), ref y);
            if (!string.IsNullOrEmpty(patient.EtatCivil.NumeroMatricule))
                AjouterLigneInfo(gfx, fontNormal, "Matricule :", patient.EtatCivil.NumeroMatricule, ref y);
            if (!string.IsNullOrEmpty(patient.EtatCivil.Grade))
                AjouterLigneInfo(gfx, fontNormal, "Grade :", patient.EtatCivil.Grade, ref y);
        }

        // Pied de page
        gfx.DrawString($"Imprimé le {DateTime.Now:dd/MM/yyyy à HH:mm}",
            new XFont("Arial", 9), XBrushes.Gray,
            new XRect(0, HauteurPage - 30, LargeurPage, 20), XStringFormats.TopCenter);
    }

    private void AjouterSectionEtatCivil(PdfDocument doc, EtatCivil ec)
    {
        var page = doc.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;

        AjouterEnTetSection(gfx, "A", "ÉTAT CIVIL", ref y);
        var font = new XFont("Arial", 11, XFontStyle.Regular);

        AjouterLigneInfo(gfx, font, "Nom :", ec.Nom.ToUpper(), ref y);
        AjouterLigneInfo(gfx, font, "Prénoms :", ec.Prenoms, ref y);
        AjouterLigneInfo(gfx, font, "Date de naissance :", ec.DateNaissance.ToString("dd/MM/yyyy"), ref y);
        AjouterLigneInfo(gfx, font, "Lieu de naissance :", ec.LieuNaissance, ref y);
        AjouterLigneInfo(gfx, font, "Sexe :", ec.Sexe.ToString(), ref y);
        AjouterLigneInfo(gfx, font, "Groupe sanguin :", ec.GroupeSanguin.ToString(), ref y);

        if (!string.IsNullOrEmpty(ec.NumeroMatricule))
            AjouterLigneInfo(gfx, font, "Matricule :", ec.NumeroMatricule, ref y);
        if (!string.IsNullOrEmpty(ec.Grade))
            AjouterLigneInfo(gfx, font, "Grade :", ec.Grade, ref y);
        if (!string.IsNullOrEmpty(ec.Unite))
            AjouterLigneInfo(gfx, font, "Unité :", ec.Unite, ref y);

        y += 10;
        AjouterSousTitre(gfx, "FILIATIONS", ref y);
        if (!string.IsNullOrEmpty(ec.NomPere))
            AjouterLigneInfo(gfx, font, "Père :", $"{ec.NomPere} {ec.PrenomsPere}", ref y);
        if (!string.IsNullOrEmpty(ec.NomMere))
            AjouterLigneInfo(gfx, font, "Mère :", $"{ec.NomMere} {ec.PrenomsMere}", ref y);

        if (ec.ContactsUrgence.Any())
        {
            y += 10;
            AjouterSousTitre(gfx, "CONTACTS D'URGENCE", ref y);
            foreach (var contact in ec.ContactsUrgence)
                AjouterLigneInfo(gfx, font, contact.LienParente ?? "Contact :",
                    $"{contact.NomComplet} - Tél: {contact.Telephone}", ref y);
        }

        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionConstantes(PdfDocument doc, List<Constante> constantes)
    {
        var page = doc.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;

        AjouterEnTetSection(gfx, "B", "CONSTANTES MÉDICALES", ref y);

        // En-tête tableau
        var fontEnt = new XFont("Arial", 9, XFontStyle.Bold);
        var fontData = new XFont("Arial", 9, XFontStyle.Regular);
        double[] cols = { 60, 50, 40, 40, 55, 55, 45, 45, 50 };
        string[] headers = { "Date", "Taille", "Poids", "IMC", "T.Systol.", "T.Diast.", "Glycémie", "Albumine", "Médecin" };

        double x = MargeGauche;
        for (int i = 0; i < headers.Length; i++)
        {
            gfx.DrawRectangle(XPens.Gray, XBrushes.LightGray, x, y, cols[i], 18);
            gfx.DrawString(headers[i], fontEnt, XBrushes.Black,
                new XRect(x + 2, y + 2, cols[i] - 4, 16), XStringFormats.TopLeft);
            x += cols[i];
        }
        y += 18;

        foreach (var c in constantes.Take(20))
        {
            x = MargeGauche;
            string[] vals = {
                c.DateMesure.ToString("dd/MM/yy"),
                c.Taille.HasValue ? $"{c.Taille} cm" : "-",
                c.Poids.HasValue ? $"{c.Poids} kg" : "-",
                c.IMC.HasValue ? $"{c.IMC}" : "-",
                c.TensionSystolique.HasValue ? $"{c.TensionSystolique}" : "-",
                c.TensionDiastolique.HasValue ? $"{c.TensionDiastolique}" : "-",
                c.Glycemie.HasValue ? $"{c.Glycemie}" : "-",
                c.Albumine.HasValue ? $"{c.Albumine}" : "-",
                c.MedecinMesureur ?? "-"
            };

            for (int i = 0; i < vals.Length; i++)
            {
                gfx.DrawRectangle(XPens.LightGray, x, y, cols[i], 16);
                gfx.DrawString(vals[i], fontData, XBrushes.Black,
                    new XRect(x + 2, y + 2, cols[i] - 4, 14), XStringFormats.TopLeft);
                x += cols[i];
            }
            y += 16;
            if (y > HauteurPage - 60) break;
        }

        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionExamenIncorporation(PdfDocument doc, ExamenIncorporation ei)
    {
        var page = doc.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;
        var font = new XFont("Arial", 10, XFontStyle.Regular);

        AjouterEnTetSection(gfx, "C", "EXAMEN MÉDICAL D'INCORPORATION", ref y);
        AjouterLigneInfo(gfx, font, "Date :", ei.DateExamen.ToString("dd/MM/yyyy"), ref y);
        AjouterLigneInfo(gfx, font, "Médecin :", ei.MedecinExaminateur ?? "-", ref y);

        AjouterSousTitre(gfx, "ANTÉCÉDENTS", ref y);
        AjouterChampTexte(gfx, font, "Héréditaires", ei.AntecedentsHeredita, ref y);
        AjouterChampTexte(gfx, font, "Personnels", ei.AntecedentsPersonnels, ref y);
        AjouterChampTexte(gfx, font, "Collatéraux", ei.AntecedentsCollateraux, ref y);

        AjouterSousTitre(gfx, "APPAREILS ET SYSTÈMES", ref y);
        AjouterChampTexte(gfx, font, "Respiratoire", ei.AppareilRespiratoire, ref y);
        AjouterChampTexte(gfx, font, "Digestif", ei.AppareilDigestif, ref y);
        AjouterChampTexte(gfx, font, "Circulatoire", ei.AppareilCirculatoire, ref y);
        AjouterChampTexte(gfx, font, "Génito-urinaire", ei.AppareilGenitourinaire, ref y);
        AjouterChampTexte(gfx, font, "Système nerveux", ei.SystemeNerveux, ref y);
        AjouterChampTexte(gfx, font, "Denture", ei.Denture, ref y);
        AjouterChampTexte(gfx, font, "Peau & annexes", ei.PeauAnnexes, ref y);

        AjouterSousTitre(gfx, "VISION", ref y);
        AjouterLigneInfo(gfx, font, "OD sans/avec correction :",
            $"{ei.VisionODSansCorrection ?? 0:F1} / {ei.VisionODAvecCorrection ?? 0:F1}", ref y);
        AjouterLigneInfo(gfx, font, "OG sans/avec correction :",
            $"{ei.VisionOGSansCorrection ?? 0:F1} / {ei.VisionOGAvecCorrection ?? 0:F1}", ref y);

        AjouterSousTitre(gfx, "APTITUDE", ref y);
        AjouterLigneInfo(gfx, font, "Aptitude médicale :",
            ei.AptitudeMedicale.ToString().ToUpper(), ref y);
        AjouterChampTexte(gfx, font, "Mentions spéciales", ei.MentionsMedicalesSpeciales, ref y);

        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionOperations(PdfDocument doc, List<OperationMedicale> operations)
    {
        var page = doc.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;
        var font = new XFont("Arial", 10, XFontStyle.Regular);

        AjouterEnTetSection(gfx, "D", "OPÉRATIONS MÉDICALES", ref y);

        foreach (var op in operations)
        {
            if (y > HauteurPage - 100) { AjouterPiedDePage(gfx, page.Number); break; }

            gfx.DrawRectangle(XPens.DarkBlue, XBrushes.AliceBlue, MargeGauche, y, LargeurContenu, 16);
            var fontBold = new XFont("Arial", 10, XFontStyle.Bold);
            gfx.DrawString($"  {op.DateOperation:dd/MM/yyyy} - {op.Diagnostic}",
                fontBold, XBrushes.DarkBlue,
                new XRect(MargeGauche + 2, y + 2, LargeurContenu - 4, 14), XStringFormats.TopLeft);
            y += 18;

            AjouterLigneInfo(gfx, font, "Type :", op.TypeIntervention.ToString(), ref y);
            if (!string.IsNullOrEmpty(op.LieuSejour))
                AjouterLigneInfo(gfx, font, "Lieu :", op.LieuSejour, ref y);
            AjouterChampTexte(gfx, font, "État avant", op.EtatAvant, ref y);
            AjouterChampTexte(gfx, font, "État après", op.EtatApres, ref y);
            AjouterChampTexte(gfx, font, "Observations", op.Observations, ref y);
            if (!string.IsNullOrEmpty(op.SignatureMedecin))
                AjouterLigneInfo(gfx, font, "Médecin :", op.SignatureMedecin, ref y);
            y += 8;
        }

        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionVaccinations(PdfDocument doc, List<Vaccination> vaccinations)
    {
        var page = doc.AddPage();
        page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;

        AjouterEnTetSection(gfx, "E", "VACCINATIONS ET IMMUNISATIONS", ref y);

        var fontEnt = new XFont("Arial", 9, XFontStyle.Bold);
        var fontData = new XFont("Arial", 9, XFontStyle.Regular);
        double[] cols = { 100, 90, 60, 40, 80, 80, 65 };
        string[] headers = { "Vaccin", "Type", "Date", "Dose", "N° Lot", "Médecin", "Validité" };

        double x = MargeGauche;
        for (int i = 0; i < headers.Length; i++)
        {
            gfx.DrawRectangle(XPens.Gray, XBrushes.LightBlue, x, y, cols[i], 18);
            gfx.DrawString(headers[i], fontEnt, XBrushes.Black,
                new XRect(x + 2, y + 2, cols[i] - 4, 16), XStringFormats.TopLeft);
            x += cols[i];
        }
        y += 18;

        foreach (var v in vaccinations)
        {
            x = MargeGauche;
            string[] vals = {
                v.NomVaccin,
                v.TypeVaccin.ToString(),
                v.DateVaccination.ToString("dd/MM/yyyy"),
                $"Dose {v.NumeroDose}",
                v.NumeroLot ?? "-",
                v.MedecinVaccinateur ?? "-",
                v.DateExpiration.HasValue ? v.DateExpiration.Value.ToString("MM/yyyy") : "Permanent"
            };

            for (int i = 0; i < vals.Length; i++)
            {
                gfx.DrawRectangle(XPens.LightGray, x, y, cols[i], 16);
                gfx.DrawString(vals[i], fontData, XBrushes.Black,
                    new XRect(x + 2, y + 2, cols[i] - 4, 14), XStringFormats.TopLeft);
                x += cols[i];
            }
            y += 16;
            if (y > HauteurPage - 60) break;
        }

        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionVisites(PdfDocument doc, List<VisiteSanitaire> visites)
    {
        var page = doc.AddPage(); page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;
        var font = new XFont("Arial", 10, XFontStyle.Regular);

        AjouterEnTetSection(gfx, "F", "VISITES SANITAIRES", ref y);

        foreach (var v in visites)
        {
            if (y > HauteurPage - 80) break;
            var fontBold = new XFont("Arial", 10, XFontStyle.Bold);
            gfx.DrawString($"Visite du {v.DateVisite:dd/MM/yyyy} - {v.EntiteMedicale}",
                fontBold, XBrushes.DarkBlue,
                new XRect(MargeGauche, y, LargeurContenu, 14), XStringFormats.TopLeft);
            y += 16;
            AjouterChampTexte(gfx, font, "Résultats", v.ResultatsVisite, ref y);
            AjouterChampTexte(gfx, font, "Observations", v.Observations, ref y);
            if (!string.IsNullOrEmpty(v.NomMedecin))
                AjouterLigneInfo(gfx, font, "Médecin :", v.NomMedecin, ref y);
            y += 5;
        }
        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionIndisponibilites(PdfDocument doc, List<Indisponibilite> items)
    {
        var page = doc.AddPage(); page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;
        var font = new XFont("Arial", 10, XFontStyle.Regular);

        AjouterEnTetSection(gfx, "G", "INDISPONIBILITÉS POUR RAISON DE SANTÉ", ref y);

        foreach (var item in items)
        {
            if (y > HauteurPage - 80) break;
            var fontBold = new XFont("Arial", 10, XFontStyle.Bold);
            gfx.DrawString($"Du {item.DateDebut:dd/MM/yyyy} - {item.Motif}",
                fontBold, XBrushes.DarkRed,
                new XRect(MargeGauche, y, LargeurContenu, 14), XStringFormats.TopLeft);
            y += 16;
            AjouterLigneInfo(gfx, font, "Durée :", $"{item.DureePrescrite} jours", ref y);
            AjouterChampTexte(gfx, font, "Diagnostic", item.Diagnostic, ref y);
            AjouterChampTexte(gfx, font, "État départ", item.EtatDepart, ref y);
            AjouterChampTexte(gfx, font, "État retour", item.EtatRetour, ref y);
            y += 5;
        }
        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionCertificats(PdfDocument doc, List<CertificatMedical> items)
    {
        var page = doc.AddPage(); page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;
        var font = new XFont("Arial", 10, XFontStyle.Regular);

        AjouterEnTetSection(gfx, "H", "CERTIFICATS MÉDICAUX", ref y);

        foreach (var item in items)
        {
            if (y > HauteurPage - 80) break;
            var fontBold = new XFont("Arial", 10, XFontStyle.Bold);
            gfx.DrawString($"{item.TypeCertificat} - {item.DateCertificat:dd/MM/yyyy}",
                fontBold, XBrushes.DarkGreen,
                new XRect(MargeGauche, y, LargeurContenu, 14), XStringFormats.TopLeft);
            y += 16;
            AjouterChampTexte(gfx, font, "Objet", item.Objet, ref y);
            AjouterChampTexte(gfx, font, "Contenu", item.Contenu, ref y);
            if (!string.IsNullOrEmpty(item.MedecinSignataire))
                AjouterLigneInfo(gfx, font, "Médecin signataire :", item.MedecinSignataire, ref y);
            if (!string.IsNullOrEmpty(item.FichierPath))
                AjouterLigneInfo(gfx, font, "Fichier joint :", item.FichierNom ?? "Oui", ref y);
            y += 5;
        }
        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionDecisionsReforme(PdfDocument doc, List<DecisionReforme> items)
    {
        var page = doc.AddPage(); page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;
        var font = new XFont("Arial", 10, XFontStyle.Regular);

        AjouterEnTetSection(gfx, "I", "DÉCISIONS DES COMMISSIONS DE RÉFORME", ref y);

        foreach (var item in items)
        {
            if (y > HauteurPage - 80) break;
            var fontBold = new XFont("Arial", 10, XFontStyle.Bold);
            gfx.DrawString($"N° {item.NumeroDecision} du {item.DateDecision:dd/MM/yyyy}",
                fontBold, XBrushes.DarkViolet,
                new XRect(MargeGauche, y, LargeurContenu, 14), XStringFormats.TopLeft);
            y += 16;
            AjouterLigneInfo(gfx, font, "Diagnostic :", item.Diagnostic, ref y);
            AjouterLigneInfo(gfx, font, "Décision :", item.Decision.ToString(), ref y);
            AjouterChampTexte(gfx, font, "Observations", item.Observations, ref y);
            y += 5;
        }
        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    private void AjouterSectionControleFinService(PdfDocument doc, ControleFInService controle)
    {
        var page = doc.AddPage(); page.Size = PdfSharpCore.PageSize.A4;
        using var gfx = XGraphics.FromPdfPage(page);
        double y = MargeHaut;
        var font = new XFont("Arial", 10, XFontStyle.Regular);

        AjouterEnTetSection(gfx, "J", "CONTRÔLE DE FIN DE SERVICE", ref y);
        AjouterLigneInfo(gfx, font, "Date du contrôle :", controle.DateControle.ToString("dd/MM/yyyy"), ref y);
        AjouterChampTexte(gfx, font, "Examen final", controle.ExamenFinal, ref y);
        AjouterChampTexte(gfx, font, "État de santé", controle.EtatDeSante, ref y);
        AjouterLigneInfo(gfx, font, "Aptitude au retour :", controle.AptitudeRejoindreForyer.ToString(), ref y);
        AjouterChampTexte(gfx, font, "Recommandations", controle.RecommandationsMedicales, ref y);

        if (controle.DateFinService.HasValue)
            AjouterLigneInfo(gfx, font, "Date fin de service :", controle.DateFinService.Value.ToString("dd/MM/yyyy"), ref y);
        if (controle.TotalJoursIndisponibilite.HasValue)
            AjouterLigneInfo(gfx, font, "Total jours d'indisponibilité :", controle.TotalJoursIndisponibilite.Value.ToString(), ref y);

        if (!string.IsNullOrEmpty(controle.MedecinSignataire))
        {
            y += 20;
            AjouterLigneInfo(gfx, font, "Médecin signataire :", controle.MedecinSignataire, ref y);
        }

        AjouterPiedDePage(gfx, doc.Pages.Count);
    }

    // ── Helpers de dessin ─────────────────────────────────────

    private static readonly XStringFormat _middleLeft = new XStringFormat
    {
        Alignment = XStringAlignment.Near,
        LineAlignment = XLineAlignment.Center
    };

    private void AjouterEnTetSection(XGraphics gfx, string lettre, string titre, ref double y)
    {
        var fontTitre = new XFont("Arial", 16, XFontStyle.Bold);
        var fontLettre = new XFont("Arial", 20, XFontStyle.Bold);
        gfx.DrawRectangle(XBrushes.DarkBlue, MargeGauche, y, LargeurContenu, 35);
        gfx.DrawString($" {lettre}.", fontLettre, XBrushes.White,
            new XRect(MargeGauche, y, 50, 35), _middleLeft);
        gfx.DrawString(titre, fontTitre, XBrushes.White,
            new XRect(MargeGauche + 50, y, LargeurContenu - 50, 35), _middleLeft);
        y += 45;
    }

    private void AjouterSousTitre(XGraphics gfx, string titre, ref double y)
    {
        var font = new XFont("Arial", 11, XFontStyle.Bold);
        gfx.DrawRectangle(XBrushes.LightSteelBlue, MargeGauche, y, LargeurContenu, 18);
        gfx.DrawString($"  {titre}", font, XBrushes.DarkBlue,
            new XRect(MargeGauche, y + 2, LargeurContenu, 16), XStringFormats.TopLeft);
        y += 22;
    }

    private void AjouterLigneInfo(XGraphics gfx, XFont font, string label, string valeur, ref double y)
    {
        var fontBold = new XFont("Arial", font.Size, XFontStyle.Bold);
        gfx.DrawString(label, fontBold, XBrushes.Black,
            new XRect(MargeGauche, y, 160, 14), XStringFormats.TopLeft);
        gfx.DrawString(valeur, font, XBrushes.DarkSlateGray,
            new XRect(MargeGauche + 165, y, LargeurContenu - 165, 14), XStringFormats.TopLeft);
        y += 16;
    }

    private void AjouterChampTexte(XGraphics gfx, XFont font, string label, string? valeur, ref double y)
    {
        if (string.IsNullOrWhiteSpace(valeur)) return;
        var fontBold = new XFont("Arial", font.Size, XFontStyle.Bold);
        gfx.DrawString($"{label} :", fontBold, XBrushes.Black,
            new XRect(MargeGauche, y, LargeurContenu, 14), XStringFormats.TopLeft);
        y += 14;
        // Texte sur plusieurs lignes (simple)
        gfx.DrawString(valeur.Length > 120 ? valeur[..120] + "..." : valeur, font,
            XBrushes.DarkSlateGray,
            new XRect(MargeGauche + 10, y, LargeurContenu - 10, 30), XStringFormats.TopLeft);
        y += 18;
    }

    private void AjouterPiedDePage(XGraphics gfx, int numero)
    {
        var font = new XFont("Arial", 8, XFontStyle.Italic);
        gfx.DrawLine(XPens.LightGray, MargeGauche, HauteurPage - 25, LargeurPage - MargeGauche, HauteurPage - 25);
        gfx.DrawString($"Carnet de Santé Sanitaire Officiel - Confidentiel - Page {numero}",
            font, XBrushes.Gray,
            new XRect(0, HauteurPage - 22, LargeurPage, 15), XStringFormats.TopCenter);
    }

    private static string GetNomPatient(Patient patient)
        => patient.EtatCivil != null
            ? $"{patient.EtatCivil.Nom} {patient.EtatCivil.Prenoms}"
            : patient.NumeroCarnet;
}
