using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE J - CONTRÔLE DE FIN DE SERVICE
/// Bilan médical de sortie du service actif.
/// </summary>
public class ControleFInService : BaseEntity
{
    public int PatientId { get; set; }

    public DateTime DateControle { get; set; }

    // ── Examen final ─────────────────────────────────────────
    public string? ExamenFinal { get; set; }
    public string? DiagnosticsFinaux { get; set; }
    public string? EtatDeSante { get; set; }

    // ── Aptitude au retour au foyer ──────────────────────────
    public AptitudeMedicale AptitudeRejoindreForyer { get; set; }
    public string? ConditionsRejoindreForyer { get; set; }
    public string? RecommandationsMedicales { get; set; }
    public string? TraitementsDeLongDuree { get; set; }

    // ── Fin de service ────────────────────────────────────────
    public DateTime? DateFinService { get; set; }
    public DateTime? DateRadiation { get; set; }
    public string? MotifFinService { get; set; }     // Retraite, Démission, Décès...
    public string? NumeroDecisionRadiation { get; set; }

    // ── Récapitulatif de santé ────────────────────────────────
    public string? RecapitulatifPathologies { get; set; }
    public string? RecapitulatifInterventions { get; set; }
    public string? RecapitulatifIndisponibilites { get; set; }
    public int? TotalJoursIndisponibilite { get; set; }

    // ── Médecin ───────────────────────────────────────────────
    public string? MedecinSignataire { get; set; }
    public string? CodeMedecin { get; set; }
    public string? SignatureMedecin { get; set; }
    public string? Observations { get; set; }

    // Navigation
    public Patient? Patient { get; set; }
}
