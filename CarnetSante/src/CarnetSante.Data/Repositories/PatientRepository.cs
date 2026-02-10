using CarnetSante.Core.Models;
using CarnetSante.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Repositories;

/// <summary>
/// Repository spécialisé pour les Patients avec chargement complet du carnet.
/// </summary>
public interface IPatientRepository : IRepository<Patient>
{
    Task<Patient?> GetPatientCompletAsync(int patientId);
    Task<IEnumerable<Patient>> RechercherAsync(string? nom, string? prenom, string? numeroCarnet, string? matricule);
    Task<bool> NumeroCarnetExisteAsync(string numeroCarnet, int? excludeId = null);
}

public class PatientRepository : Repository<Patient>, IPatientRepository
{
    public PatientRepository(CarnetSanteDbContext context) : base(context) { }

    /// <summary>
    /// Charge un patient avec TOUTES ses relations (carnet complet).
    /// </summary>
    public async Task<Patient?> GetPatientCompletAsync(int patientId)
    {
        return await _context.Patients
            .Include(p => p.EtatCivil)
                .ThenInclude(ec => ec!.ContactsUrgence)
            .Include(p => p.Constantes.Where(c => !c.IsDeleted).OrderByDescending(c => c.DateMesure))
            .Include(p => p.ExamenIncorporation)
            .Include(p => p.OperationsMedicales.Where(o => !o.IsDeleted).OrderByDescending(o => o.DateOperation))
            .Include(p => p.Vaccinations.Where(v => !v.IsDeleted).OrderByDescending(v => v.DateVaccination))
            .Include(p => p.VisitesSanitaires.Where(vs => !vs.IsDeleted).OrderByDescending(vs => vs.DateVisite))
            .Include(p => p.Indisponibilites.Where(i => !i.IsDeleted).OrderByDescending(i => i.DateDebut))
            .Include(p => p.CertificatsMedicaux.Where(cm => !cm.IsDeleted).OrderByDescending(cm => cm.DateCertificat))
            .Include(p => p.DecisionsReforme.Where(dr => !dr.IsDeleted).OrderByDescending(dr => dr.DateDecision))
            .Include(p => p.ControleFinService)
            .FirstOrDefaultAsync(p => p.Id == patientId && !p.IsDeleted);
    }

    /// <summary>
    /// Recherche multicritères sur les patients.
    /// </summary>
    public async Task<IEnumerable<Patient>> RechercherAsync(
        string? nom, string? prenom, string? numeroCarnet, string? matricule)
    {
        var query = _context.Patients
            .Include(p => p.EtatCivil)
            .Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(numeroCarnet))
            query = query.Where(p => p.NumeroCarnet.Contains(numeroCarnet));

        if (!string.IsNullOrWhiteSpace(nom))
            query = query.Where(p => p.EtatCivil != null &&
                p.EtatCivil.Nom.ToLower().Contains(nom.ToLower()));

        if (!string.IsNullOrWhiteSpace(prenom))
            query = query.Where(p => p.EtatCivil != null &&
                p.EtatCivil.Prenoms.ToLower().Contains(prenom.ToLower()));

        if (!string.IsNullOrWhiteSpace(matricule))
            query = query.Where(p => p.EtatCivil != null &&
                p.EtatCivil.NumeroMatricule != null &&
                p.EtatCivil.NumeroMatricule.Contains(matricule));

        return await query
            .OrderBy(p => p.EtatCivil != null ? p.EtatCivil.Nom : p.NumeroCarnet)
            .ToListAsync();
    }

    public async Task<bool> NumeroCarnetExisteAsync(string numeroCarnet, int? excludeId = null)
    {
        var query = _context.Patients.Where(p => p.NumeroCarnet == numeroCarnet && !p.IsDeleted);
        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);
        return await query.AnyAsync();
    }
}
