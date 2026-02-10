using System.Collections.ObjectModel;

namespace CarnetSante.Core.Models;

/// <summary>
/// Table PATIENTS - Entité centrale du carnet de santé.
/// Correspond à l'en-tête et l'identifiant unique du carnet.
/// ObservableCollection utilisé sur toutes les collections pour que les DataGrids WPF
/// se rafraîchissent immédiatement après ajout via les dialogs.
/// </summary>
public class Patient : BaseEntity
{
    public string NumeroCarnet { get; set; } = string.Empty; // Numéro unique du carnet
    public string? PhotoPath { get; set; }

    // ── MODULE A : ÉTAT CIVIL ──────────────────────────────────
    public EtatCivil? EtatCivil { get; set; }

    // ── MODULE B : CONSTANTES ──────────────────────────────────
    public ObservableCollection<Constante> Constantes { get; set; } = [];

    // ── MODULE C : EXAMEN D'INCORPORATION ─────────────────────
    public ExamenIncorporation? ExamenIncorporation { get; set; }

    // ── MODULE D : OPÉRATIONS MÉDICALES ───────────────────────
    public ObservableCollection<OperationMedicale> OperationsMedicales { get; set; } = [];

    // ── MODULE E : VACCINATIONS ────────────────────────────────
    public ObservableCollection<Vaccination> Vaccinations { get; set; } = [];

    // ── MODULE F : VISITES SANITAIRES ─────────────────────────
    public ObservableCollection<VisiteSanitaire> VisitesSanitaires { get; set; } = [];

    // ── MODULE G : INDISPONIBILITÉS ────────────────────────────
    public ObservableCollection<Indisponibilite> Indisponibilites { get; set; } = [];

    // ── MODULE H : CERTIFICATS MÉDICAUX ───────────────────────
    public ObservableCollection<CertificatMedical> CertificatsMedicaux { get; set; } = [];

    // ── MODULE I : DÉCISIONS DE RÉFORME ───────────────────────
    public ObservableCollection<DecisionReforme> DecisionsReforme { get; set; } = [];

    // ── MODULE J : CONTRÔLE FIN DE SERVICE ────────────────────
    public ControleFInService? ControleFinService { get; set; }
}
