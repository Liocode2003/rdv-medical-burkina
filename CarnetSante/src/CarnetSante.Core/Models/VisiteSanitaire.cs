namespace CarnetSante.Core.Models;

public class VisiteSanitaire : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateVisite { get; set; }
    public string Motif { get; set; } = string.Empty;
    public string Etablissement { get; set; } = string.Empty;
    public string MedecinTraitant { get; set; } = string.Empty;
    public string Diagnostic { get; set; } = string.Empty;
    public string Traitement { get; set; } = string.Empty;
    public int? DureeHospitalisation { get; set; } // jours
    public string Suites { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;

    // Dossier d'hospitalisation détaillé
    public string HistoireMaladie { get; set; } = string.Empty;
    public string EtatGeneral { get; set; } = string.Empty;
    public string ExamenCardiovasculaire { get; set; } = string.Empty;
    public string ExamenPleuroPulmonaire { get; set; } = string.Empty;
    public string ExamenDigestif { get; set; } = string.Empty;
    public string ExamenNeurologique { get; set; } = string.Empty;
    public string ExamenAutresAppareils { get; set; } = string.Empty;
    public string ExamensBiologiques { get; set; } = string.Empty;
    public string ExamensRadiologiques { get; set; } = string.Empty;
    public string EvolutionClinique { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
