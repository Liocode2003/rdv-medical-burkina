namespace CarnetSante.Core.Models;

public class ContactUrgence : BaseEntity
{
    public int PatientId { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string LienParente { get; set; } = string.Empty;
    public string Telephone { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;

    public Patient? Patient { get; set; }
}
