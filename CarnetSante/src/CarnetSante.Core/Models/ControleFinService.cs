using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

public class ControleFinService : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateControle { get; set; }
    public string MedecinControleur { get; set; } = string.Empty;
    public Aptitude AptitudeFinal { get; set; }
    public string BilanSante { get; set; } = string.Empty;
    public string PathologiesChroniques { get; set; } = string.Empty;
    public string TraitementsEnCours { get; set; } = string.Empty;
    public string RecommandationsSante { get; set; } = string.Empty;
    public string Conclusion { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
