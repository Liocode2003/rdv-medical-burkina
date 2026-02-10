using CarnetSante.Core.Models;
using CarnetSante.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Repositories;

/// <summary>
/// Repository spécialisé pour la gestion des utilisateurs et l'authentification.
/// </summary>
public interface IUtilisateurRepository : IRepository<Utilisateur>
{
    Task<Utilisateur?> GetByLoginAsync(string login);
    Task<bool> LoginExisteAsync(string login, int? excludeId = null);
    Task IncrémenterTentativesEchecAsync(int userId);
    Task RéinitialiserTentativesAsync(int userId);
    Task MettreAJourDerniereConnexionAsync(int userId);
}

public class UtilisateurRepository : Repository<Utilisateur>, IUtilisateurRepository
{
    public UtilisateurRepository(CarnetSanteDbContext context) : base(context) { }

    public async Task<Utilisateur?> GetByLoginAsync(string login)
        => await _dbSet.FirstOrDefaultAsync(u => u.Login == login && !u.IsDeleted && u.EstActif);

    public async Task<bool> LoginExisteAsync(string login, int? excludeId = null)
    {
        var query = _dbSet.Where(u => u.Login == login && !u.IsDeleted);
        if (excludeId.HasValue)
            query = query.Where(u => u.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task IncrémenterTentativesEchecAsync(int userId)
    {
        var user = await _dbSet.FindAsync(userId);
        if (user != null)
        {
            user.TentativesEchec++;
            if (user.TentativesEchec >= 5)
                user.BloquéJusquau = DateTime.UtcNow.AddMinutes(30);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RéinitialiserTentativesAsync(int userId)
    {
        var user = await _dbSet.FindAsync(userId);
        if (user != null)
        {
            user.TentativesEchec = 0;
            user.BloquéJusquau = null;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MettreAJourDerniereConnexionAsync(int userId)
    {
        var user = await _dbSet.FindAsync(userId);
        if (user != null)
        {
            user.DerniereConnexion = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
