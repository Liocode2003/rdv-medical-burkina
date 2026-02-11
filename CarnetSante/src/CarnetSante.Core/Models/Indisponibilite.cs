namespace CarnetSante.Core.Models;

public class Indisponibilite : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }
    public string TypeIndisponibilite { get; set; } = string.Empty;
    public string Motif { get; set; } = string.Empty;
    public string AutoriteDecision { get; set; } = string.Empty;
    public string ReferenceDocument { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;

    public int? DureeJours => DateFin.HasValue
        ? (int)(DateFin.Value - DateDebut).TotalDays
        : null;

    public Patient? Patient { get; set; }
}
