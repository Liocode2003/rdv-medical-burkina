using CarnetSante.Core.Enums;
using CarnetSante.Core.Models;

namespace CarnetSante.Core.Services;

/// <summary>
/// Interface du service de traçabilité/audit.
/// </summary>
public interface IAuditService
{
    Task EnregistrerAsync(
        TypeAction typeAction,
        string? description = null,
        string? entite = null,
        int? idEntite = null,
        int? patientId = null,
        string? anciennesValeurs = null,
        string? nouvellesValeurs = null);

    Task<IEnumerable<JournalAudit>> GetJournalAsync(int page = 1, int taille = 50);
    Task<IEnumerable<JournalAudit>> GetJournalPatientAsync(int patientId);
}
