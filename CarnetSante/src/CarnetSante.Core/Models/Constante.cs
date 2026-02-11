namespace CarnetSante.Core.Models;

public class Constante : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateMesure { get; set; } = DateTime.Now;
    public decimal? Poids { get; set; }           // kg
    public decimal? Taille { get; set; }           // cm
    public decimal? IMC { get; set; }              // calculé automatiquement
    public string? TensionArterielle { get; set; } // ex: 120/80
    public int? FrequenceCardiaque { get; set; }   // bpm
    public int? FrequenceRespiratoire { get; set; }// /min
    public decimal? Temperature { get; set; }      // °C
    public decimal? Glycemie { get; set; }         // g/L
    public string? Observations { get; set; }
    public string MedecinResponsable { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
