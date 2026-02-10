using CarnetSante.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CarnetSante.Data.Context;

/// <summary>
/// DbContext principal de l'application CarnetSanté.
/// Configure toutes les tables et relations EF Core.
/// </summary>
public class CarnetSanteDbContext : DbContext
{
    public CarnetSanteDbContext(DbContextOptions<CarnetSanteDbContext> options)
        : base(options) { }

    // ── Tables principales ────────────────────────────────────
    public DbSet<Utilisateur> Utilisateurs { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<JournalAudit> JournalAudits { get; set; }

    // ── Modules A → J ─────────────────────────────────────────
    public DbSet<EtatCivil> EtatsCivils { get; set; }
    public DbSet<ContactUrgence> ContactsUrgence { get; set; }
    public DbSet<Constante> Constantes { get; set; }
    public DbSet<ExamenIncorporation> ExamensIncorporation { get; set; }
    public DbSet<OperationMedicale> OperationsMedicales { get; set; }
    public DbSet<Vaccination> Vaccinations { get; set; }
    public DbSet<VisiteSanitaire> VisitesSanitaires { get; set; }
    public DbSet<Indisponibilite> Indisponibilites { get; set; }
    public DbSet<CertificatMedical> CertificatsMedicaux { get; set; }
    public DbSet<DecisionReforme> DecisionsReforme { get; set; }
    public DbSet<ControleFInService> ControlesFinService { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ── UTILISATEUR ──────────────────────────────────────
        modelBuilder.Entity<Utilisateur>(e =>
        {
            e.HasKey(u => u.Id);
            e.HasIndex(u => u.Login).IsUnique();
            e.Property(u => u.Login).IsRequired().HasMaxLength(50);
            e.Property(u => u.Nom).IsRequired().HasMaxLength(100);
            e.Property(u => u.Prenom).IsRequired().HasMaxLength(100);
            e.Property(u => u.MotDePasseHash).IsRequired();
            e.Property(u => u.Role).HasConversion<int>();
        });

        // ── PATIENT ───────────────────────────────────────────
        modelBuilder.Entity<Patient>(e =>
        {
            e.HasKey(p => p.Id);
            e.HasIndex(p => p.NumeroCarnet).IsUnique();
            e.Property(p => p.NumeroCarnet).IsRequired().HasMaxLength(30);

            // Relations 1-1
            e.HasOne(p => p.EtatCivil)
             .WithOne(ec => ec.Patient)
             .HasForeignKey<EtatCivil>(ec => ec.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(p => p.ExamenIncorporation)
             .WithOne(ei => ei.Patient)
             .HasForeignKey<ExamenIncorporation>(ei => ei.PatientId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(p => p.ControleFinService)
             .WithOne(c => c.Patient)
             .HasForeignKey<ControleFInService>(c => c.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── ÉTAT CIVIL ───────────────────────────────────────
        modelBuilder.Entity<EtatCivil>(e =>
        {
            e.HasKey(ec => ec.Id);
            e.Property(ec => ec.Nom).IsRequired().HasMaxLength(100);
            e.Property(ec => ec.Prenoms).IsRequired().HasMaxLength(200);
            e.Property(ec => ec.LieuNaissance).IsRequired().HasMaxLength(200);
            e.Property(ec => ec.Sexe).HasConversion<int>();
            e.Property(ec => ec.GroupeSanguin).HasConversion<int>();

            e.HasMany(ec => ec.ContactsUrgence)
             .WithOne(cu => cu.EtatCivil)
             .HasForeignKey(cu => cu.EtatCivilId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── CONTACT URGENCE ──────────────────────────────────
        modelBuilder.Entity<ContactUrgence>(e =>
        {
            e.HasKey(cu => cu.Id);
            e.Property(cu => cu.NomComplet).IsRequired().HasMaxLength(200);
        });

        // ── CONSTANTES ───────────────────────────────────────
        modelBuilder.Entity<Constante>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Taille).HasPrecision(5, 1);
            e.Property(c => c.Poids).HasPrecision(5, 1);
            e.Property(c => c.IMC).HasPrecision(5, 2);
            e.Property(c => c.Glycemie).HasPrecision(5, 2);
            e.Property(c => c.Albumine).HasPrecision(5, 2);
            e.HasOne(c => c.Patient)
             .WithMany(p => p.Constantes)
             .HasForeignKey(c => c.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── EXAMEN INCORPORATION ─────────────────────────────
        modelBuilder.Entity<ExamenIncorporation>(e =>
        {
            e.HasKey(ei => ei.Id);
            e.Property(ei => ei.AptitudeMedicale).HasConversion<int>();
        });

        // ── OPÉRATIONS MÉDICALES ─────────────────────────────
        modelBuilder.Entity<OperationMedicale>(e =>
        {
            e.HasKey(o => o.Id);
            e.Property(o => o.Diagnostic).IsRequired().HasMaxLength(500);
            e.Property(o => o.TypeIntervention).HasConversion<int>();
            e.HasOne(o => o.Patient)
             .WithMany(p => p.OperationsMedicales)
             .HasForeignKey(o => o.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── VACCINATIONS ─────────────────────────────────────
        modelBuilder.Entity<Vaccination>(e =>
        {
            e.HasKey(v => v.Id);
            e.Property(v => v.NomVaccin).IsRequired().HasMaxLength(200);
            e.Property(v => v.TypeVaccin).HasConversion<int>();
            e.HasOne(v => v.Patient)
             .WithMany(p => p.Vaccinations)
             .HasForeignKey(v => v.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── VISITES SANITAIRES ───────────────────────────────
        modelBuilder.Entity<VisiteSanitaire>(e =>
        {
            e.HasKey(vs => vs.Id);
            e.Property(vs => vs.EntiteMedicale).IsRequired().HasMaxLength(200);
            e.HasOne(vs => vs.Patient)
             .WithMany(p => p.VisitesSanitaires)
             .HasForeignKey(vs => vs.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── INDISPONIBILITÉS ─────────────────────────────────
        modelBuilder.Entity<Indisponibilite>(e =>
        {
            e.HasKey(i => i.Id);
            e.Property(i => i.Motif).IsRequired().HasMaxLength(500);
            e.Property(i => i.Statut).HasConversion<int>();
            e.HasOne(i => i.Patient)
             .WithMany(p => p.Indisponibilites)
             .HasForeignKey(i => i.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── CERTIFICATS MÉDICAUX ─────────────────────────────
        modelBuilder.Entity<CertificatMedical>(e =>
        {
            e.HasKey(cm => cm.Id);
            e.Property(cm => cm.TypeCertificat).IsRequired().HasMaxLength(200);
            e.HasOne(cm => cm.Patient)
             .WithMany(p => p.CertificatsMedicaux)
             .HasForeignKey(cm => cm.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── DÉCISIONS RÉFORME ────────────────────────────────
        modelBuilder.Entity<DecisionReforme>(e =>
        {
            e.HasKey(dr => dr.Id);
            e.Property(dr => dr.Diagnostic).IsRequired().HasMaxLength(500);
            e.Property(dr => dr.Decision).HasConversion<int>();
            e.HasOne(dr => dr.Patient)
             .WithMany(p => p.DecisionsReforme)
             .HasForeignKey(dr => dr.PatientId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ── CONTRÔLE FIN DE SERVICE ──────────────────────────
        modelBuilder.Entity<ControleFInService>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.AptitudeRejoindreForyer).HasConversion<int>();
        });

        // ── JOURNAL AUDIT ────────────────────────────────────
        modelBuilder.Entity<JournalAudit>(e =>
        {
            e.HasKey(j => j.Id);
            e.Property(j => j.TypeAction).HasConversion<int>();
            e.HasOne(j => j.Utilisateur)
             .WithMany(u => u.JournalAudits)
             .HasForeignKey(j => j.UtilisateurId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ── SEED : Administrateur par défaut ─────────────────
        modelBuilder.Entity<Utilisateur>().HasData(new Utilisateur
        {
            Id = 1,
            Login = "admin",
            // Hash BCrypt de "Admin@2024!" - à changer à la première connexion
            MotDePasseHash = "$2a$11$XqBhJzN5Wz0vNcPFqM9VCuJ8K7LmZrT3eYpA1dR4xGsHbWkO6vI2m",
            Nom = "Administrateur",
            Prenom = "Système",
            Role = CarnetSante.Core.Enums.UserRole.Administrateur,
            EstActif = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
