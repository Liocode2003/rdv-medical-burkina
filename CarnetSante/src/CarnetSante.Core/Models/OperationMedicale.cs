using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE D - OPÉRATIONS MÉDICALES
/// Historique des interventions chirurgicales et médicales.
/// </summary>
public class OperationMedicale : BaseEntity
{
    public int PatientId { get; set; }

    public DateTime DateOperation { get; set; }
    public string Diagnostic { get; set; } = string.Empty;
    public TypeIntervention TypeIntervention { get; set; }
    public string? DescriptionIntervention { get; set; }

    // ── Lieu de séjour ───────────────────────────────────────
    public string? LieuSejour { get; set; }         // Hôpital, clinique
    public DateTime? DateAdmission { get; set; }
    public DateTime? DateSortie { get; set; }
    public int? DureeSejour { get; set; }           // En jours (calculé)

    // ── État clinique ────────────────────────────────────────
    public string? EtatAvant { get; set; }
    public string? EtatApres { get; set; }
    public string? Complications { get; set; }
    public string? SuitesDonnees { get; set; }       // Prescriptions post-op

    // ── Équipe médicale ──────────────────────────────────────
    public string? ChirurgienPrincipal { get; set; }
    public string? EquipeMedicale { get; set; }
    public string? Observations { get; set; }
    public string? SignatureMedecin { get; set; }
    public string? CodeMedecin { get; set; }

    // ── Documents joints ─────────────────────────────────────
    public string? CompteRenduPath { get; set; }    // Chemin vers le fichier CR

    // Navigation
    public Patient? Patient { get; set; }
}
