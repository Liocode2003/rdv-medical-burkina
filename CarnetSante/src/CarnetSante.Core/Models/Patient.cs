using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// Table PATIENTS - Entité centrale du carnet de santé.
/// Correspond à l'en-tête et l'identifiant unique du carnet.
/// </summary>
public class Patient : BaseEntity
{
    public string NumeroCarnet { get; set; } = string.Empty; // Numéro unique du carnet
    public string? PhotoPath { get; set; }

    // ── MODULE A : ÉTAT CIVIL ──────────────────────────────────
    public EtatCivil? EtatCivil { get; set; }

    // ── MODULE B : CONSTANTES ──────────────────────────────────
    public ICollection<Constante> Constantes { get; set; } = new List<Constante>();

    // ── MODULE C : EXAMEN D'INCORPORATION ─────────────────────
    public ExamenIncorporation? ExamenIncorporation { get; set; }

    // ── MODULE D : OPÉRATIONS MÉDICALES ───────────────────────
    public ICollection<OperationMedicale> OperationsMedicales { get; set; } = new List<OperationMedicale>();

    // ── MODULE E : VACCINATIONS ────────────────────────────────
    public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();

    // ── MODULE F : VISITES SANITAIRES ─────────────────────────
    public ICollection<VisiteSanitaire> VisitesSanitaires { get; set; } = new List<VisiteSanitaire>();

    // ── MODULE G : INDISPONIBILITÉS ────────────────────────────
    public ICollection<Indisponibilite> Indisponibilites { get; set; } = new List<Indisponibilite>();

    // ── MODULE H : CERTIFICATS MÉDICAUX ───────────────────────
    public ICollection<CertificatMedical> CertificatsMedicaux { get; set; } = new List<CertificatMedical>();

    // ── MODULE I : DÉCISIONS DE RÉFORME ───────────────────────
    public ICollection<DecisionReforme> DecisionsReforme { get; set; } = new List<DecisionReforme>();

    // ── MODULE J : CONTRÔLE FIN DE SERVICE ────────────────────
    public ControleFInService? ControleFinService { get; set; }
}
