using CarnetSante.Core.Enums;
using System.Collections.ObjectModel;

namespace CarnetSante.Core.Models;

/// <summary>
/// MODULE A - ÉTAT CIVIL DU PATIENT
/// Correspond à la section A du carnet sanitaire officiel.
/// </summary>
public class EtatCivil : BaseEntity
{
    public int PatientId { get; set; }

    // ── Identité principale ──────────────────────────────────
    public string Nom { get; set; } = string.Empty;
    public string Prenoms { get; set; } = string.Empty;
    public DateTime DateNaissance { get; set; }
    public string LieuNaissance { get; set; } = string.Empty;
    public string? PaysNaissance { get; set; }
    public Sexe Sexe { get; set; }
    public GroupeSanguin GroupeSanguin { get; set; } = GroupeSanguin.Inconnu;
    public string? Nationalite { get; set; }

    // ── Filiations ──────────────────────────────────────────
    public string? NomPere { get; set; }
    public string? PrenomsPere { get; set; }
    public string? ProfessionPere { get; set; }
    public string? NomMere { get; set; }
    public string? PrenomsMere { get; set; }
    public string? ProfessionMere { get; set; }
    public string? SituationMatrimoniale { get; set; }
    public int? NombreEnfants { get; set; }

    // ── Identifiants administratifs ─────────────────────────
    public string? NumeroMatricule { get; set; }
    public string? NumeroCNI { get; set; }
    public string? NumeroPasseport { get; set; }
    public string? Corps { get; set; }        // Ex: Armée, Gendarmerie...
    public string? Grade { get; set; }
    public string? Unite { get; set; }
    public string? Fonction { get; set; }
    public DateTime? DateRecrutement { get; set; }

    // ── Coordonnées ─────────────────────────────────────────
    public string? Adresse { get; set; }
    public string? Ville { get; set; }
    public string? Telephone { get; set; }
    public string? Email { get; set; }

    // ── Contacts d'urgence ──────────────────────────────────
    public ObservableCollection<ContactUrgence> ContactsUrgence { get; set; } = [];

    // ── Empreintes (symbolique) ─────────────────────────────
    public string? EmpreintesNotes { get; set; }
    public string? EmpreintesImagePath { get; set; }

    // Navigation
    public Patient? Patient { get; set; }
}
