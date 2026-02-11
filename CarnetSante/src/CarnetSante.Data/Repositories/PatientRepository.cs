using CarnetSante.Core.Interfaces;
using CarnetSante.Core.Models;
using CarnetSante.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Patient>> GetAllAsync(string? searchTerm = null)
    {
        var query = _context.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            query = query.Where(p =>
                p.Nom.ToLower().Contains(term) ||
                p.Prenoms.ToLower().Contains(term) ||
                p.NumeroCarnet.ToLower().Contains(term) ||
                p.Matricule.ToLower().Contains(term));
        }

        return await query
            .OrderBy(p => p.Nom)
            .ThenBy(p => p.Prenoms)
            .ToListAsync();
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.EtatCivil)
            .Include(p => p.ContactsUrgence)
            .Include(p => p.Constantes)
            .Include(p => p.ExamenIncorporation)
            .Include(p => p.OperationsMedicales)
            .Include(p => p.Vaccinations)
            .Include(p => p.VisitesSanitaires)
            .Include(p => p.Indisponibilites)
            .Include(p => p.CertificatsMedicaux)
            .Include(p => p.DecisionsReforme)
            .Include(p => p.ControleFinService)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Patient?> GetByNumeroCarnetAsync(string numeroCarnet)
    {
        return await _context.Patients
            .FirstOrDefaultAsync(p => p.NumeroCarnet == numeroCarnet);
    }

    public async Task<Patient> CreateAsync(Patient patient)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task<Patient> UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync();
        return patient;
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient != null)
        {
            patient.IsDeleted = true;
            patient.DeletedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetCountAsync()
    {
        return await _context.Patients.CountAsync();
    }

    public async Task<bool> ExistsAsync(string numeroCarnet, int? excludeId = null)
    {
        var query = _context.Patients.Where(p => p.NumeroCarnet == numeroCarnet);
        if (excludeId.HasValue)
            query = query.Where(p => p.Id != excludeId.Value);
        return await query.AnyAsync();
    }
}
