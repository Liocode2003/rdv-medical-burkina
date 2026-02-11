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

    public Patient? Patient { get; set; }
}
