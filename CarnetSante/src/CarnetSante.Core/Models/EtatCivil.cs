namespace CarnetSante.Core.Models;

public class EtatCivil : BaseEntity
{
    public int PatientId { get; set; }
    public string SituationMatrimoniale { get; set; } = string.Empty;
    public string NomPere { get; set; } = string.Empty;
    public string NomMere { get; set; } = string.Empty;
    public int NombreEnfants { get; set; }
    public string Profession { get; set; } = string.Empty;
    public string Corps { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public string Affectation { get; set; } = string.Empty;
    public string ServiceOrigine { get; set; } = string.Empty;
    public DateTime? DateIntegration { get; set; }
    public string Observations { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
