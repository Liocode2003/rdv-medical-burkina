namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE F - VISITES SANITAIRES PÉRIODIQUES
/// Contrôles médicaux réguliers (annuels, semestriels, etc.).
/// </summary>
public class VisiteSanitaire : BaseEntity
{
    public int PatientId { get; set; }

    public DateTime DateVisite { get; set; }
    public string? TypeVisite { get; set; }         // Annuelle, semestrielle, circonstancielle
    public string EntiteMedicale { get; set; } = string.Empty; // Structure de santé

    // ── Résultats de la visite ───────────────────────────────
    public string? ResultatsVisite { get; set; }
    public string? DiagnosticsRetenus { get; set; }
    public string? ExamensPrescrits { get; set; }
    public string? TraitementsPrescrits { get; set; }

    // ── Aptitude à l'issue de la visite ─────────────────────
    public string? AptitudeConclusionVisite { get; set; }
    public DateTime? ProchainContrôle { get; set; }

    // ── Médecin ───────────────────────────────────────────────
    public string? Observations { get; set; }
    public string? SignatureMedecin { get; set; }
    public string? CodeMedecin { get; set; }
    public string? NomMedecin { get; set; }

    // Navigation
    public Patient? Patient { get; set; }
}
