using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

public class ExamenIncorporation : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateExamen { get; set; }
    public Aptitude Aptitude { get; set; }
    public string MedecinExaminateur { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
    public string AntecedentsMedicaux { get; set; } = string.Empty;
    public string AntecedentsChirurgicaux { get; set; } = string.Empty;
    public string AntecedentsFamiliaux { get; set; } = string.Empty;
    public string ExamenClinique { get; set; } = string.Empty;
    public string ExamensComplementaires { get; set; } = string.Empty;
    public string Conclusion { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
