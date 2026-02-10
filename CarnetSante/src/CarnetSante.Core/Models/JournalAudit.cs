using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// TABLE JOURNAL_AUDIT - Traçabilité complète de toutes les actions.
/// Immuable : ne doit jamais être modifié ou supprimé.
/// </summary>
public class JournalAudit
{
    public int Id { get; set; }
    public DateTime DateAction { get; set; } = DateTime.UtcNow;

    public int? UtilisateurId { get; set; }
    public string? LoginUtilisateur { get; set; }
    public TypeAction TypeAction { get; set; }

    public string? EntiteAffectee { get; set; }    // Nom de la table/entité
    public int? IdEntiteAffectee { get; set; }      // ID de l'enregistrement
    public int? PatientId { get; set; }             // Patient concerné si applicable

    public string? Description { get; set; }
    public string? AnciennesValeurs { get; set; }   // JSON des anciennes valeurs
    public string? NouvellesValeurs { get; set; }   // JSON des nouvelles valeurs

    public string? AdresseIP { get; set; }
    public string? NomMachine { get; set; }

    // Navigation
    public Utilisateur? Utilisateur { get; set; }
}
