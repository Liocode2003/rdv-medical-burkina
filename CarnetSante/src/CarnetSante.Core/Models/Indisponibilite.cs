using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE G - INDISPONIBILITÉS POUR RAISON DE SANTÉ
/// Périodes d'arrêt de service pour maladie, blessure ou convalescence.
/// </summary>
public class Indisponibilite : BaseEntity
{
    public int PatientId { get; set; }

    public DateTime DateDebut { get; set; }
    public DateTime? DateFin { get; set; }

    // ── Durée ─────────────────────────────────────────────────
    /// <summary>Durée prescrite initialement (en jours).</summary>
    public int DureePrescrite { get; set; }

    /// <summary>Durée réelle (calculée ou saisie).</summary>
    public int? DureeReelle { get; set; }

    public StatutIndisponibilite Statut { get; set; } = StatutIndisponibilite.EnCours;

    // ── Cause ─────────────────────────────────────────────────
    public string Motif { get; set; } = string.Empty;
    public string? Diagnostic { get; set; }
    public string? CodeCIM10 { get; set; }          // Code CIM-10 du diagnostic

    // ── Lieu de séjour ───────────────────────────────────────
    public string? LieuSejour { get; set; }
    public string? TypeSejour { get; set; }         // Hospitalisation, domicile, convalescence

    // ── État clinique ────────────────────────────────────────
    public string? EtatDepart { get; set; }         // État en partant en arrêt
    public string? EtatRetour { get; set; }         // État au retour

    // ── Médecin ───────────────────────────────────────────────
    public string? MedecinPrescripteur { get; set; }
    public string? Observations { get; set; }
    public string? SignatureMedecin { get; set; }

    // Navigation
    public Patient? Patient { get; set; }
}
