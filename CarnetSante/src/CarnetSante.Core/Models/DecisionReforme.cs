using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE I - DÉCISIONS DES COMMISSIONS DE RÉFORME
/// Décisions officielles concernant l'aptitude au service.
/// </summary>
public class DecisionReforme : BaseEntity
{
    public int PatientId { get; set; }

    public DateTime DateDecision { get; set; }
    public string NumeroDecision { get; set; } = string.Empty; // Référence officielle

    // ── Composition de la commission ────────────────────────
    public string? CompositionCommission { get; set; }
    public string? LieuCommission { get; set; }
    public DateTime? DateReunionCommission { get; set; }

    // ── Diagnostic retenu ────────────────────────────────────
    public string Diagnostic { get; set; } = string.Empty;
    public string? CodeCIM10 { get; set; }
    public string? AffectionsPrincipales { get; set; }
    public string? AffectionsAssociees { get; set; }
    public string? TauxInvalidite { get; set; }     // Ex: 25%, 40%...

    // ── Décision ─────────────────────────────────────────────
    public TypeDecisionReforme Decision { get; set; }
    public string? MotifDecision { get; set; }
    public string? ConditionsReengagement { get; set; }
    public bool PensionAttribuee { get; set; } = false;
    public string? TypePension { get; set; }
    public decimal? TauxPension { get; set; }

    // ── Suite ─────────────────────────────────────────────────
    public string Observations { get; set; } = string.Empty;
    public DateTime? DateEffet { get; set; }

    // Navigation
    public Patient? Patient { get; set; }
}
