using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

public class Vaccination : BaseEntity
{
    public int PatientId { get; set; }
    public TypeVaccin TypeVaccin { get; set; }
    public string NomVaccin { get; set; } = string.Empty;
    public DateTime DateVaccination { get; set; }
    public DateTime? DateRappel { get; set; }
    public string Lot { get; set; } = string.Empty;
    public string Centre { get; set; } = string.Empty;
    public string Operateur { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
