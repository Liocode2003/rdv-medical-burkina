using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

public class OperationMedicale : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateOperation { get; set; }
    public string Intitule { get; set; } = string.Empty;
    public TypeIntervention TypeIntervention { get; set; }
    public string Chirurgien { get; set; } = string.Empty;
    public string Etablissement { get; set; } = string.Empty;
    public string Diagnostic { get; set; } = string.Empty;
    public string Technique { get; set; } = string.Empty;
    public string Suites { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
