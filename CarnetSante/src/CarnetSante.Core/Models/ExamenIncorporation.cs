using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE C - EXAMEN MÉDICAL D'INCORPORATION
/// Bilan médical complet réalisé à l'entrée en service.
/// </summary>
public class ExamenIncorporation : BaseEntity
{
    public int PatientId { get; set; }
    public DateTime DateExamen { get; set; }
    public string? MedecinExaminateur { get; set; }

    // ── ANTÉCÉDENTS ──────────────────────────────────────────
    public string? AntecedentsHeredita { get; set; }   // Antécédents héréditaires
    public string? AntecedentsPersonnels { get; set; } // Antécédents personnels
    public string? AntecedentsCollateraux { get; set; } // Antécédents collatéraux (fratrie)
    public string? AllergiesConnues { get; set; }
    public string? TraitementsEnCours { get; set; }

    // ── APPAREILS ────────────────────────────────────────────
    public string? AppareilRespiratoire { get; set; }
    public string? AppareilDigestif { get; set; }
    public string? AppareilCirculatoire { get; set; }
    public string? AppareilGenitourinaire { get; set; }
    public string? SystemeNerveux { get; set; }
    public string? SystemeOsteoarticulaire { get; set; }
    public string? SystemeEndocrinien { get; set; }

    // ── EXAMEN SPÉCIALISÉ ─────────────────────────────────────
    public string? Denture { get; set; }
    public string? PeauAnnexes { get; set; }

    // ── VISION ───────────────────────────────────────────────
    public decimal? VisionODSansCorrection { get; set; }  // Œil droit sans correction
    public decimal? VisionOGSansCorrection { get; set; }  // Œil gauche sans correction
    public decimal? VisionODAvecCorrection { get; set; }  // Œil droit avec correction
    public decimal? VisionOGAvecCorrection { get; set; }  // Œil gauche avec correction
    public string? TypeCorrectionOD { get; set; }          // Ex: Myopie, Hypermétropie
    public string? TypeCorrectionOG { get; set; }
    public decimal? EcartPupillaire { get; set; }          // En mm
    public string? ObservationsVisuelles { get; set; }

    // ── AUDITION ─────────────────────────────────────────────
    public string? AuditionODVoixHaute { get; set; }      // Distance d'audition voix haute OD
    public string? AuditionOGVoixHaute { get; set; }
    public string? AuditionODVoixChuchote { get; set; }   // Distance voix chuchotée OD
    public string? AuditionOGVoixChuchote { get; set; }
    public string? ObservationsAuditives { get; set; }

    // ── SENS CHROMATIQUE ──────────────────────────────────────
    public bool? SensChromatique { get; set; }             // true = normal
    public string? TypeDaltonisme { get; set; }
    public string? ObservationsSensChromatique { get; set; }

    // ── APTITUDE MÉDICALE ─────────────────────────────────────
    public AptitudeMedicale AptitudeMedicale { get; set; } = AptitudeMedicale.Apte;
    public string? MentionsMedicalesSpeciales { get; set; }
    public string? RestrictionsActivite { get; set; }
    public string? Observations { get; set; }
    public string? Signature { get; set; }

    // Navigation
    public Patient? Patient { get; set; }
}
