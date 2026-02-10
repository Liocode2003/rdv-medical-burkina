using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;
using CarnetSante.Core.Services;
using CarnetSante.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Repositories;

/// <summary>
/// Implémentation du service d'audit/traçabilité.
/// </summary>
public class AuditService : IAuditService
{
    private readonly CarnetSanteDbContext _context;
    private readonly IAuthService _authService;

    public AuditService(CarnetSanteDbContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task EnregistrerAsync(
        TypeAction typeAction,
        string? description = null,
        string? entite = null,
        int? idEntite = null,
        int? patientId = null,
        string? anciennesValeurs = null,
        string? nouvellesValeurs = null)
    {
        var user = _authService.UtilisateurCourant;

        var journal = new JournalAudit
        {
            DateAction = DateTime.UtcNow,
            UtilisateurId = user?.Id,
            LoginUtilisateur = user?.Login,
            TypeAction = typeAction,
            EntiteAffectee = entite,
            IdEntiteAffectee = idEntite,
            PatientId = patientId,
            Description = description,
            AnciennesValeurs = anciennesValeurs,
            NouvellesValeurs = nouvellesValeurs,
            AdresseIP = GetLocalIP(),
            NomMachine = Environment.MachineName
        };

        await _context.JournalAudits.AddAsync(journal);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<JournalAudit>> GetJournalAsync(int page = 1, int taille = 50)
    {
        return await _context.JournalAudits
            .Include(j => j.Utilisateur)
            .OrderByDescending(j => j.DateAction)
            .Skip((page - 1) * taille)
            .Take(taille)
            .ToListAsync();
    }

    public async Task<IEnumerable<JournalAudit>> GetJournalPatientAsync(int patientId)
    {
        return await _context.JournalAudits
            .Include(j => j.Utilisateur)
            .Where(j => j.PatientId == patientId)
            .OrderByDescending(j => j.DateAction)
            .ToListAsync();
    }

    private static string GetLocalIP()
    {
        try
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            return host.AddressList
                .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                ?.ToString() ?? "127.0.0.1";
        }
        catch { return "127.0.0.1"; }
    }
}
