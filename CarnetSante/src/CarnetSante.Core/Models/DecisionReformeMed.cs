using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

public class DecisionReformeMed : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateDecision { get; set; }
    public DecisionReforme Decision { get; set; }
    public string MotifDecision { get; set; } = string.Empty;
    public string CommissionMedicale { get; set; } = string.Empty;
    public string ReferenceActe { get; set; } = string.Empty;
    public Aptitude AptitudeConstatee { get; set; }
    public string Observations { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
