namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE H - CERTIFICATS MÉDICAUX
/// Certificats d'origine de blessures ou maladies avec pièces jointes.
/// </summary>
public class CertificatMedical : BaseEntity
{
    public int PatientId { get; set; }

    public DateTime DateCertificat { get; set; }
    public string TypeCertificat { get; set; } = string.Empty; // Origine blessure, aptitude, etc.
    public string? Objet { get; set; }                 // Objet du certificat
    public string? Contenu { get; set; }               // Texte du certificat

    // ── Nature ────────────────────────────────────────────────
    public string? OrigineBlessureOuMaladie { get; set; }  // Civile, militaire, accident...
    public bool? ImputabiliteService { get; set; }         // Lié au service ?
    public string? CirconstancesOrigine { get; set; }

    // ── Médecin signataire ───────────────────────────────────
    public string? MedecinSignataire { get; set; }
    public string? CodeMedecin { get; set; }
    public string? Etablissement { get; set; }
    public string? NumeroOrdre { get; set; }           // Numéro ordre médecins

    // ── Fichier joint ────────────────────────────────────────
    public string? FichierPath { get; set; }           // Chemin vers scan ou PDF
    public string? FichierNom { get; set; }
    public long? FichierTaille { get; set; }           // En octets
    public string? FichierType { get; set; }           // PDF, JPG, PNG, etc.

    public string? Observations { get; set; }

    // Navigation
    public Patient? Patient { get; set; }
}
