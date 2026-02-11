using CarnetSante.Core.Enums;

namespace CarnetSante.Core.Models;

/// <summary>
/// Informations de session de l'utilisateur actuellement connecté.
/// </summary>
public class SessionUtilisateur
{
    public int UtilisateurId { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string NomComplet => $"{Prenom} {Nom}".Trim();
    public UserRole Role { get; set; }
    public string? CodeMedecin { get; set; }
    public string? Specialite { get; set; }
    public DateTime DebutSession { get; set; } = DateTime.UtcNow;

    public static SessionUtilisateur FromUtilisateur(Utilisateur u) => new()
    {
        UtilisateurId = u.Id,
        Login = u.Login,
        Nom = u.Nom,
        Prenom = u.Prenom,
        Role = u.Role,
        CodeMedecin = u.CodeMedecin,
        Specialite = u.Specialite,
        DebutSession = DateTime.UtcNow
    };
}
