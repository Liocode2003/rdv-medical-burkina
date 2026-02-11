using CarnetSante.Core.Interfaces;
using CarnetSante.Core.Models;
using CarnetSante.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Repositories;

public class ConstanteRepository : IConstanteRepository
{
    private readonly AppDbContext _ctx;
    public ConstanteRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<Constante>> GetByPatientAsync(int patientId) =>
        await _ctx.Constantes.Where(c => c.PatientId == patientId)
            .OrderByDescending(c => c.DateMesure).ToListAsync();

    public async Task<Constante> SaveAsync(Constante c)
    {
        if (c.Id == 0) _ctx.Constantes.Add(c); else _ctx.Constantes.Update(c);
        await _ctx.SaveChangesAsync(); return c;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.Constantes.FindAsync(id);
        if (e != null) { e.IsDeleted = true; await _ctx.SaveChangesAsync(); }
    }
}

public class EtatCivilRepository : IEtatCivilRepository
{
    private readonly AppDbContext _ctx;
    public EtatCivilRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<EtatCivil?> GetByPatientAsync(int patientId) =>
        await _ctx.EtatsCivils.FirstOrDefaultAsync(e => e.PatientId == patientId);

    public async Task<EtatCivil> SaveAsync(EtatCivil e)
    {
        if (e.Id == 0) _ctx.EtatsCivils.Add(e); else _ctx.EtatsCivils.Update(e);
        await _ctx.SaveChangesAsync(); return e;
    }
}

public class ContactUrgenceRepository : IContactUrgenceRepository
{
    private readonly AppDbContext _ctx;
    public ContactUrgenceRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<ContactUrgence>> GetByPatientAsync(int patientId) =>
        await _ctx.ContactsUrgence.Where(c => c.PatientId == patientId).ToListAsync();

    public async Task<ContactUrgence> SaveAsync(ContactUrgence c)
    {
        if (c.Id == 0) _ctx.ContactsUrgence.Add(c); else _ctx.ContactsUrgence.Update(c);
        await _ctx.SaveChangesAsync(); return c;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.ContactsUrgence.FindAsync(id);
        if (e != null) { e.IsDeleted = true; await _ctx.SaveChangesAsync(); }
    }
}

public class ExamenIncorporationRepository : IExamenIncorporationRepository
{
    private readonly AppDbContext _ctx;
    public ExamenIncorporationRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<ExamenIncorporation?> GetByPatientAsync(int patientId) =>
        await _ctx.ExamensIncorporation.FirstOrDefaultAsync(e => e.PatientId == patientId);

    public async Task<ExamenIncorporation> SaveAsync(ExamenIncorporation e)
    {
        if (e.Id == 0) _ctx.ExamensIncorporation.Add(e); else _ctx.ExamensIncorporation.Update(e);
        await _ctx.SaveChangesAsync(); return e;
    }
}

public class OperationMedicaleRepository : IOperationMedicaleRepository
{
    private readonly AppDbContext _ctx;
    public OperationMedicaleRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<OperationMedicale>> GetByPatientAsync(int patientId) =>
        await _ctx.OperationsMedicales.Where(o => o.PatientId == patientId)
            .OrderByDescending(o => o.DateOperation).ToListAsync();

    public async Task<OperationMedicale> SaveAsync(OperationMedicale o)
    {
        if (o.Id == 0) _ctx.OperationsMedicales.Add(o); else _ctx.OperationsMedicales.Update(o);
        await _ctx.SaveChangesAsync(); return o;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.OperationsMedicales.FindAsync(id);
        if (e != null) { e.IsDeleted = true; await _ctx.SaveChangesAsync(); }
    }
}

public class VaccinationRepository : IVaccinationRepository
{
    private readonly AppDbContext _ctx;
    public VaccinationRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<Vaccination>> GetByPatientAsync(int patientId) =>
        await _ctx.Vaccinations.Where(v => v.PatientId == patientId)
            .OrderByDescending(v => v.DateVaccination).ToListAsync();

    public async Task<Vaccination> SaveAsync(Vaccination v)
    {
        if (v.Id == 0) _ctx.Vaccinations.Add(v); else _ctx.Vaccinations.Update(v);
        await _ctx.SaveChangesAsync(); return v;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.Vaccinations.FindAsync(id);
        if (e != null) { e.IsDeleted = true; await _ctx.SaveChangesAsync(); }
    }
}

public class VisiteSanitaireRepository : IVisiteSanitaireRepository
{
    private readonly AppDbContext _ctx;
    public VisiteSanitaireRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<VisiteSanitaire>> GetByPatientAsync(int patientId) =>
        await _ctx.VisitesSanitaires.Where(v => v.PatientId == patientId)
            .OrderByDescending(v => v.DateVisite).ToListAsync();

    public async Task<VisiteSanitaire> SaveAsync(VisiteSanitaire v)
    {
        if (v.Id == 0) _ctx.VisitesSanitaires.Add(v); else _ctx.VisitesSanitaires.Update(v);
        await _ctx.SaveChangesAsync(); return v;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.VisitesSanitaires.FindAsync(id);
        if (e != null) { e.IsDeleted = true; await _ctx.SaveChangesAsync(); }
    }
}

public class IndisponibiliteRepository : IIndisponibiliteRepository
{
    private readonly AppDbContext _ctx;
    public IndisponibiliteRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<Indisponibilite>> GetByPatientAsync(int patientId) =>
        await _ctx.Indisponibilites.Where(i => i.PatientId == patientId)
            .OrderByDescending(i => i.DateDebut).ToListAsync();

    public async Task<Indisponibilite> SaveAsync(Indisponibilite i)
    {
        if (i.Id == 0) _ctx.Indisponibilites.Add(i); else _ctx.Indisponibilites.Update(i);
        await _ctx.SaveChangesAsync(); return i;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.Indisponibilites.FindAsync(id);
        if (e != null) { e.IsDeleted = true; await _ctx.SaveChangesAsync(); }
    }
}

public class CertificatMedicalRepository : ICertificatMedicalRepository
{
    private readonly AppDbContext _ctx;
    public CertificatMedicalRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<CertificatMedical>> GetByPatientAsync(int patientId) =>
        await _ctx.CertificatsMedicaux.Where(c => c.PatientId == patientId)
            .OrderByDescending(c => c.DateCertificat).ToListAsync();

    public async Task<CertificatMedical> SaveAsync(CertificatMedical c)
    {
        if (c.Id == 0) _ctx.CertificatsMedicaux.Add(c); else _ctx.CertificatsMedicaux.Update(c);
        await _ctx.SaveChangesAsync(); return c;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.CertificatsMedicaux.FindAsync(id);
        if (e != null) { e.IsDeleted = true; await _ctx.SaveChangesAsync(); }
    }
}

public class DecisionReformeRepository : IDecisionReformeRepository
{
    private readonly AppDbContext _ctx;
    public DecisionReformeRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<List<DecisionReformeMed>> GetByPatientAsync(int patientId) =>
        await _ctx.DecisionsReforme.Where(d => d.PatientId == patientId)
            .OrderByDescending(d => d.DateDecision).ToListAsync();

    public async Task<DecisionReformeMed> SaveAsync(DecisionReformeMed d)
    {
        if (d.Id == 0) _ctx.DecisionsReforme.Add(d); else _ctx.DecisionsReforme.Update(d);
        await _ctx.SaveChangesAsync(); return d;
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _ctx.DecisionsReforme.FindAsync(id);
        if (e != null) { e.IsDeleted = true; await _ctx.SaveChangesAsync(); }
    }
}

public class ControleFinServiceRepository : IControleFinServiceRepository
{
    private readonly AppDbContext _ctx;
    public ControleFinServiceRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task<ControleFinService?> GetByPatientAsync(int patientId) =>
        await _ctx.ControlesFinService.FirstOrDefaultAsync(c => c.PatientId == patientId);

    public async Task<ControleFinService> SaveAsync(ControleFinService c)
    {
        if (c.Id == 0) _ctx.ControlesFinService.Add(c); else _ctx.ControlesFinService.Update(c);
        await _ctx.SaveChangesAsync(); return c;
    }
}
