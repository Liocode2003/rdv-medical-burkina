using CarnetSante.Core.Models;

namespace CarnetSante.Core.Services;

/// <summary>
/// Interface du service de génération PDF.
/// </summary>
public interface IPdfService
{
    /// <summary>Génère le carnet complet en PDF.</summary>
    Task<byte[]> GenererCarnetCompletAsync(Patient patient);

    /// <summary>Génère une section spécifique du carnet en PDF.</summary>
    Task<byte[]> GenererSectionAsync(Patient patient, string section);

    /// <summary>Sauvegarde le PDF dans un fichier.</summary>
    Task SauvegarderPdfAsync(byte[] pdfData, string cheminFichier);

    /// <summary>Ouvre le PDF dans le visualiseur par défaut.</summary>
    Task OuvrirPdfAsync(byte[] pdfData, string nomFichierTemp);
}
