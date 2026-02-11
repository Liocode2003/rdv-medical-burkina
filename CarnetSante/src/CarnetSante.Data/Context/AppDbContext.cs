using CarnetSante.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<EtatCivil> EtatsCivils => Set<EtatCivil>();
    public DbSet<ContactUrgence> ContactsUrgence => Set<ContactUrgence>();
    public DbSet<Constante> Constantes => Set<Constante>();
    public DbSet<ExamenIncorporation> ExamensIncorporation => Set<ExamenIncorporation>();
    public DbSet<OperationMedicale> OperationsMedicales => Set<OperationMedicale>();
    public DbSet<Vaccination> Vaccinations => Set<Vaccination>();
    public DbSet<VisiteSanitaire> VisitesSanitaires => Set<VisiteSanitaire>();
    public DbSet<Indisponibilite> Indisponibilites => Set<Indisponibilite>();
    public DbSet<CertificatMedical> CertificatsMedicaux => Set<CertificatMedical>();
    public DbSet<DecisionReformeMed> DecisionsReforme => Set<DecisionReformeMed>();
    public DbSet<ControleFinService> ControlesFinService => Set<ControleFinService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global query filter for soft delete
        modelBuilder.Entity<Patient>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EtatCivil>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ContactUrgence>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Constante>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ExamenIncorporation>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<OperationMedicale>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Vaccination>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<VisiteSanitaire>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Indisponibilite>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<CertificatMedical>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<DecisionReformeMed>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ControleFinService>().HasQueryFilter(e => !e.IsDeleted);

        // Patient
        modelBuilder.Entity<Patient>(e =>
        {
            e.HasIndex(p => p.NumeroCarnet).IsUnique();
            e.HasIndex(p => p.Matricule);
            e.Property(p => p.Nom).HasMaxLength(100).IsRequired();
            e.Property(p => p.Prenoms).HasMaxLength(200).IsRequired();
            e.Property(p => p.NumeroCarnet).HasMaxLength(50).IsRequired();

            e.HasOne(p => p.EtatCivil)
             .WithOne(e => e.Patient)
             .HasForeignKey<EtatCivil>(e => e.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(p => p.ExamenIncorporation)
             .WithOne(e => e.Patient)
             .HasForeignKey<ExamenIncorporation>(e => e.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(p => p.ControleFinService)
             .WithOne(c => c.Patient)
             .HasForeignKey<ControleFinService>(c => c.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(p => p.ContactsUrgence)
             .WithOne(c => c.Patient)
             .HasForeignKey(c => c.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(p => p.Constantes)
             .WithOne(c => c.Patient)
             .HasForeignKey(c => c.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(p => p.OperationsMedicales)
             .WithOne(o => o.Patient)
             .HasForeignKey(o => o.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(p => p.Vaccinations)
             .WithOne(v => v.Patient)
             .HasForeignKey(v => v.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(p => p.VisitesSanitaires)
             .WithOne(v => v.Patient)
             .HasForeignKey(v => v.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(p => p.Indisponibilites)
             .WithOne(i => i.Patient)
             .HasForeignKey(i => i.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(p => p.CertificatsMedicaux)
             .WithOne(c => c.Patient)
             .HasForeignKey(c => c.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(p => p.DecisionsReforme)
             .WithOne(d => d.Patient)
             .HasForeignKey(d => d.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is CarnetSante.Core.Models.BaseEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (CarnetSante.Core.Models.BaseEntity)entry.Entity;
            if (entry.State == EntityState.Added)
                entity.CreatedAt = DateTime.Now;
            else
                entity.UpdatedAt = DateTime.Now;
        }
    }
}
