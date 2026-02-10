using BCrypt.Net;
using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using CarnetSante.Core.Services;

namespace CarnetSante.Data.Repositories;

/// <summary>
/// Implémentation du service d'authentification avec BCrypt.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUtilisateurRepository _utilisateurRepo;
    private Utilisateur? _utilisateurCourant;

    public AuthService(IUtilisateurRepository utilisateurRepo)
    {
        _utilisateurRepo = utilisateurRepo;
    }

    public Utilisateur? UtilisateurCourant => _utilisateurCourant;

    public async Task<Utilisateur?> ConnecterAsync(string login, string motDePasse)
    {
        var utilisateur = await _utilisateurRepo.GetByLoginAsync(login);

        if (utilisateur == null)
            return null;

        // Vérifier si le compte est bloqué
        if (utilisateur.BloquéJusquau.HasValue && utilisateur.BloquéJusquau > DateTime.UtcNow)
            throw new InvalidOperationException(
                $"Compte bloqué jusqu'au {utilisateur.BloquéJusquau:dd/MM/yyyy HH:mm}. Trop de tentatives échouées.");

        // Vérifier le mot de passe
        bool motDePasseValide = BCrypt.Net.BCrypt.Verify(motDePasse, utilisateur.MotDePasseHash);

        if (!motDePasseValide)
        {
            await _utilisateurRepo.IncrémenterTentativesEchecAsync(utilisateur.Id);
            return null;
        }

        // Connexion réussie
        await _utilisateurRepo.RéinitialiserTentativesAsync(utilisateur.Id);
        await _utilisateurRepo.MettreAJourDerniereConnexionAsync(utilisateur.Id);

        _utilisateurCourant = utilisateur;
        return utilisateur;
    }

    public async Task DeconnecterAsync()
    {
        _utilisateurCourant = null;
        await Task.CompletedTask;
    }

    public bool ARole(params UserRole[] roles)
    {
        if (_utilisateurCourant == null) return false;
        return roles.Contains(_utilisateurCourant.Role);
    }

    public string HacherMotDePasse(string motDePasse)
        => BCrypt.Net.BCrypt.HashPassword(motDePasse, workFactor: 12);

    public bool VerifierMotDePasse(string motDePasse, string hash)
        => BCrypt.Net.BCrypt.Verify(motDePasse, hash);

    public async Task<bool> ChangerMotDePasseAsync(int userId, string ancienMdp, string nouveauMdp)
    {
        var user = await _utilisateurRepo.GetByIdAsync(userId);
        if (user == null) return false;

        if (!VerifierMotDePasse(ancienMdp, user.MotDePasseHash))
            return false;

        user.MotDePasseHash = HacherMotDePasse(nouveauMdp);
        await _utilisateurRepo.UpdateAsync(user);
        return true;
    }
}
