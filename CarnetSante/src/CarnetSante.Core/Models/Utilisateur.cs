using CarnetSante.Core.Enums;
using System.Collections.ObjectModel;

namespace CarnetSante.Core.Models;

/// <summary>
/// Table UTILISATEURS - Gestion des comptes d'accès au système.
/// </summary>
public class Utilisateur : BaseEntity
{
    public string Login { get; set; } = string.Empty;
    public string MotDePasseHash { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public UserRole Role { get; set; }
    public bool EstActif { get; set; } = true;
    public DateTime? DerniereConnexion { get; set; }
    public int TentativesEchec { get; set; } = 0;
    public DateTime? BloquéJusquau { get; set; }
    public string? CodeMedecin { get; set; }
    public string? Specialite { get; set; }

    // Navigation
    public ObservableCollection<JournalAudit> JournalAudits { get; set; } = [];
}
