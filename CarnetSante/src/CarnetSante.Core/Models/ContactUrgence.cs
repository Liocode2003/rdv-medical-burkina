namespace CarnetSante.Core.Models;

/// <summary>
/// Contacts à prévenir en cas d'urgence (lié à EtatCivil).
/// </summary>
public class ContactUrgence : BaseEntity
{
    public int EtatCivilId { get; set; }

    public string NomComplet { get; set; } = string.Empty;
    public string? LienParente { get; set; }   // Père, Mère, Conjoint, etc.
    public string? Telephone { get; set; }
    public string? TelephoneAlternatif { get; set; }
    public string? Adresse { get; set; }
    public bool EstPrioritaire { get; set; } = false;

    // Navigation
    public EtatCivil? EtatCivil { get; set; }
}
