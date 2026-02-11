using CarnetSante.Core.Models;

namespace CarnetSante.Core.Services;

/// <summary>
/// Interface du service d'authentification.
/// </summary>
public interface IAuthService
{
    /// <summary>Authentifie un utilisateur. Retourne l'utilisateur ou null si échec.</summary>
    Task<Utilisateur?> ConnecterAsync(string login, string motDePasse);

    /// <summary>Déconnecte l'utilisateur courant.</summary>
    Task DeconnecterAsync();

    /// <summary>Retourne l'utilisateur actuellement connecté.</summary>
    Utilisateur? UtilisateurCourant { get; }

    /// <summary>Retourne les informations de session de l'utilisateur connecté.</summary>
    SessionUtilisateur? SessionCourante { get; }

    /// <summary>Vérifie si l'utilisateur courant a un rôle suffisant.</summary>
    bool ARole(params CarnetSante.Core.Enums.UserRole[] roles);

    /// <summary>Hache un mot de passe.</summary>
    string HacherMotDePasse(string motDePasse);

    /// <summary>Vérifie un mot de passe contre son hash.</summary>
    bool VerifierMotDePasse(string motDePasse, string hash);

    /// <summary>Change le mot de passe d'un utilisateur.</summary>
    Task<bool> ChangerMotDePasseAsync(int userId, string ancienMdp, string nouveauMdp);
}
