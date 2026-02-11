using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarnetSante.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NumeroCarnet = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    PhotoPath = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MotDePasseHash = table.Column<string>(type: "TEXT", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Prenom = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    EstActif = table.Column<bool>(type: "INTEGER", nullable: false),
                    DerniereConnexion = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TentativesEchec = table.Column<int>(type: "INTEGER", nullable: false),
                    BloquéJusquau = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CodeMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    Specialite = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificatsMedicaux",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCertificat = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TypeCertificat = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Objet = table.Column<string>(type: "TEXT", nullable: true),
                    Contenu = table.Column<string>(type: "TEXT", nullable: true),
                    OrigineBlessureOuMaladie = table.Column<string>(type: "TEXT", nullable: true),
                    ImputabiliteService = table.Column<bool>(type: "INTEGER", nullable: true),
                    CirconstancesOrigine = table.Column<string>(type: "TEXT", nullable: true),
                    MedecinSignataire = table.Column<string>(type: "TEXT", nullable: true),
                    CodeMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    Etablissement = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroOrdre = table.Column<string>(type: "TEXT", nullable: true),
                    FichierPath = table.Column<string>(type: "TEXT", nullable: true),
                    FichierNom = table.Column<string>(type: "TEXT", nullable: true),
                    FichierTaille = table.Column<long>(type: "INTEGER", nullable: true),
                    FichierType = table.Column<string>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificatsMedicaux", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificatsMedicaux_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Constantes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateMesure = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Taille = table.Column<decimal>(type: "TEXT", precision: 5, scale: 1, nullable: true),
                    Poids = table.Column<decimal>(type: "TEXT", precision: 5, scale: 1, nullable: true),
                    IMC = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: true),
                    PerimethreThoracique = table.Column<decimal>(type: "TEXT", nullable: true),
                    PerimetreAbdominal = table.Column<decimal>(type: "TEXT", nullable: true),
                    TensionSystolique = table.Column<int>(type: "INTEGER", nullable: true),
                    TensionDiastolique = table.Column<int>(type: "INTEGER", nullable: true),
                    FrequenceCardiaque = table.Column<int>(type: "INTEGER", nullable: true),
                    FrequenceRespiratoire = table.Column<int>(type: "INTEGER", nullable: true),
                    Temperature = table.Column<decimal>(type: "TEXT", nullable: true),
                    Glycemie = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: true),
                    UnitéGlycemie = table.Column<string>(type: "TEXT", nullable: true),
                    Albumine = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: true),
                    Spo2 = table.Column<decimal>(type: "TEXT", nullable: true),
                    MedecinMesureur = table.Column<string>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Constantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Constantes_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ControlesFinService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateControle = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExamenFinal = table.Column<string>(type: "TEXT", nullable: true),
                    DiagnosticsFinaux = table.Column<string>(type: "TEXT", nullable: true),
                    EtatDeSante = table.Column<string>(type: "TEXT", nullable: true),
                    AptitudeRejoindreForyer = table.Column<int>(type: "INTEGER", nullable: false),
                    ConditionsRejoindreForyer = table.Column<string>(type: "TEXT", nullable: true),
                    RecommandationsMedicales = table.Column<string>(type: "TEXT", nullable: true),
                    TraitementsDeLongDuree = table.Column<string>(type: "TEXT", nullable: true),
                    DateFinService = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateRadiation = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MotifFinService = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroDecisionRadiation = table.Column<string>(type: "TEXT", nullable: true),
                    RecapitulatifPathologies = table.Column<string>(type: "TEXT", nullable: true),
                    RecapitulatifInterventions = table.Column<string>(type: "TEXT", nullable: true),
                    RecapitulatifIndisponibilites = table.Column<string>(type: "TEXT", nullable: true),
                    TotalJoursIndisponibilite = table.Column<int>(type: "INTEGER", nullable: true),
                    MedecinSignataire = table.Column<string>(type: "TEXT", nullable: true),
                    CodeMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    SignatureMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlesFinService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlesFinService_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DecisionsReforme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateDecision = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NumeroDecision = table.Column<string>(type: "TEXT", nullable: false),
                    CompositionCommission = table.Column<string>(type: "TEXT", nullable: true),
                    LieuCommission = table.Column<string>(type: "TEXT", nullable: true),
                    DateReunionCommission = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Diagnostic = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CodeCIM10 = table.Column<string>(type: "TEXT", nullable: true),
                    AffectionsPrincipales = table.Column<string>(type: "TEXT", nullable: true),
                    AffectionsAssociees = table.Column<string>(type: "TEXT", nullable: true),
                    TauxInvalidite = table.Column<string>(type: "TEXT", nullable: true),
                    Decision = table.Column<int>(type: "INTEGER", nullable: false),
                    MotifDecision = table.Column<string>(type: "TEXT", nullable: true),
                    ConditionsReengagement = table.Column<string>(type: "TEXT", nullable: true),
                    PensionAttribuee = table.Column<bool>(type: "INTEGER", nullable: false),
                    TypePension = table.Column<string>(type: "TEXT", nullable: true),
                    TauxPension = table.Column<decimal>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: false),
                    DateEffet = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisionsReforme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DecisionsReforme_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EtatsCivils",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Nom = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Prenoms = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LieuNaissance = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PaysNaissance = table.Column<string>(type: "TEXT", nullable: true),
                    Sexe = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupeSanguin = table.Column<int>(type: "INTEGER", nullable: false),
                    Nationalite = table.Column<string>(type: "TEXT", nullable: true),
                    NomPere = table.Column<string>(type: "TEXT", nullable: true),
                    PrenomsPere = table.Column<string>(type: "TEXT", nullable: true),
                    ProfessionPere = table.Column<string>(type: "TEXT", nullable: true),
                    NomMere = table.Column<string>(type: "TEXT", nullable: true),
                    PrenomsMere = table.Column<string>(type: "TEXT", nullable: true),
                    ProfessionMere = table.Column<string>(type: "TEXT", nullable: true),
                    SituationMatrimoniale = table.Column<string>(type: "TEXT", nullable: true),
                    NombreEnfants = table.Column<int>(type: "INTEGER", nullable: true),
                    NumeroMatricule = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroCNI = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroPasseport = table.Column<string>(type: "TEXT", nullable: true),
                    Corps = table.Column<string>(type: "TEXT", nullable: true),
                    Grade = table.Column<string>(type: "TEXT", nullable: true),
                    Unite = table.Column<string>(type: "TEXT", nullable: true),
                    Fonction = table.Column<string>(type: "TEXT", nullable: true),
                    DateRecrutement = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Adresse = table.Column<string>(type: "TEXT", nullable: true),
                    Ville = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    EmpreintesNotes = table.Column<string>(type: "TEXT", nullable: true),
                    EmpreintesImagePath = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtatsCivils", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EtatsCivils_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamensIncorporation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateExamen = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MedecinExaminateur = table.Column<string>(type: "TEXT", nullable: true),
                    AntecedentsHeredita = table.Column<string>(type: "TEXT", nullable: true),
                    AntecedentsPersonnels = table.Column<string>(type: "TEXT", nullable: true),
                    AntecedentsCollateraux = table.Column<string>(type: "TEXT", nullable: true),
                    AllergiesConnues = table.Column<string>(type: "TEXT", nullable: true),
                    TraitementsEnCours = table.Column<string>(type: "TEXT", nullable: true),
                    AppareilRespiratoire = table.Column<string>(type: "TEXT", nullable: true),
                    AppareilDigestif = table.Column<string>(type: "TEXT", nullable: true),
                    AppareilCirculatoire = table.Column<string>(type: "TEXT", nullable: true),
                    AppareilGenitourinaire = table.Column<string>(type: "TEXT", nullable: true),
                    SystemeNerveux = table.Column<string>(type: "TEXT", nullable: true),
                    SystemeOsteoarticulaire = table.Column<string>(type: "TEXT", nullable: true),
                    SystemeEndocrinien = table.Column<string>(type: "TEXT", nullable: true),
                    Denture = table.Column<string>(type: "TEXT", nullable: true),
                    PeauAnnexes = table.Column<string>(type: "TEXT", nullable: true),
                    VisionODSansCorrection = table.Column<decimal>(type: "TEXT", nullable: true),
                    VisionOGSansCorrection = table.Column<decimal>(type: "TEXT", nullable: true),
                    VisionODAvecCorrection = table.Column<decimal>(type: "TEXT", nullable: true),
                    VisionOGAvecCorrection = table.Column<decimal>(type: "TEXT", nullable: true),
                    TypeCorrectionOD = table.Column<string>(type: "TEXT", nullable: true),
                    TypeCorrectionOG = table.Column<string>(type: "TEXT", nullable: true),
                    EcartPupillaire = table.Column<decimal>(type: "TEXT", nullable: true),
                    ObservationsVisuelles = table.Column<string>(type: "TEXT", nullable: true),
                    AuditionODVoixHaute = table.Column<string>(type: "TEXT", nullable: true),
                    AuditionOGVoixHaute = table.Column<string>(type: "TEXT", nullable: true),
                    AuditionODVoixChuchote = table.Column<string>(type: "TEXT", nullable: true),
                    AuditionOGVoixChuchote = table.Column<string>(type: "TEXT", nullable: true),
                    ObservationsAuditives = table.Column<string>(type: "TEXT", nullable: true),
                    SensChromatique = table.Column<bool>(type: "INTEGER", nullable: true),
                    TypeDaltonisme = table.Column<string>(type: "TEXT", nullable: true),
                    ObservationsSensChromatique = table.Column<string>(type: "TEXT", nullable: true),
                    AptitudeMedicale = table.Column<int>(type: "INTEGER", nullable: false),
                    MentionsMedicalesSpeciales = table.Column<string>(type: "TEXT", nullable: true),
                    RestrictionsActivite = table.Column<string>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: true),
                    Signature = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamensIncorporation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamensIncorporation_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Indisponibilites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateFin = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DureePrescrite = table.Column<int>(type: "INTEGER", nullable: false),
                    DureeReelle = table.Column<int>(type: "INTEGER", nullable: true),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false),
                    Motif = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Diagnostic = table.Column<string>(type: "TEXT", nullable: true),
                    CodeCIM10 = table.Column<string>(type: "TEXT", nullable: true),
                    LieuSejour = table.Column<string>(type: "TEXT", nullable: true),
                    TypeSejour = table.Column<string>(type: "TEXT", nullable: true),
                    EtatDepart = table.Column<string>(type: "TEXT", nullable: true),
                    EtatRetour = table.Column<string>(type: "TEXT", nullable: true),
                    MedecinPrescripteur = table.Column<string>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: true),
                    SignatureMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indisponibilites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Indisponibilites_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationsMedicales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOperation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Diagnostic = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    TypeIntervention = table.Column<int>(type: "INTEGER", nullable: false),
                    DescriptionIntervention = table.Column<string>(type: "TEXT", nullable: true),
                    LieuSejour = table.Column<string>(type: "TEXT", nullable: true),
                    DateAdmission = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateSortie = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DureeSejour = table.Column<int>(type: "INTEGER", nullable: true),
                    EtatAvant = table.Column<string>(type: "TEXT", nullable: true),
                    EtatApres = table.Column<string>(type: "TEXT", nullable: true),
                    Complications = table.Column<string>(type: "TEXT", nullable: true),
                    SuitesDonnees = table.Column<string>(type: "TEXT", nullable: true),
                    ChirurgienPrincipal = table.Column<string>(type: "TEXT", nullable: true),
                    EquipeMedicale = table.Column<string>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: true),
                    SignatureMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    CodeMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    CompteRenduPath = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationsMedicales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationsMedicales_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vaccinations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeVaccin = table.Column<int>(type: "INTEGER", nullable: false),
                    NomVaccin = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FabricantVaccin = table.Column<string>(type: "TEXT", nullable: true),
                    NumeroLot = table.Column<string>(type: "TEXT", nullable: true),
                    Reference = table.Column<string>(type: "TEXT", nullable: true),
                    DateVaccination = table.Column<DateTime>(type: "TEXT", nullable: false),
                    NumeroDose = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeDose = table.Column<string>(type: "TEXT", nullable: true),
                    QuantitéDose = table.Column<decimal>(type: "TEXT", nullable: true),
                    VoieAdministration = table.Column<string>(type: "TEXT", nullable: true),
                    SiteInjection = table.Column<string>(type: "TEXT", nullable: true),
                    ProchaineInjectionPrevue = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateExpiration = table.Column<DateTime>(type: "TEXT", nullable: true),
                    EstValide = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReactionPostVaccinale = table.Column<string>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: true),
                    MedecinVaccinateur = table.Column<string>(type: "TEXT", nullable: true),
                    CodeMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    Etablissement = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaccinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaccinations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitesSanitaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateVisite = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TypeVisite = table.Column<string>(type: "TEXT", nullable: true),
                    EntiteMedicale = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ResultatsVisite = table.Column<string>(type: "TEXT", nullable: true),
                    DiagnosticsRetenus = table.Column<string>(type: "TEXT", nullable: true),
                    ExamensPrescrits = table.Column<string>(type: "TEXT", nullable: true),
                    TraitementsPrescrits = table.Column<string>(type: "TEXT", nullable: true),
                    AptitudeConclusionVisite = table.Column<string>(type: "TEXT", nullable: true),
                    ProchainContrôle = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Observations = table.Column<string>(type: "TEXT", nullable: true),
                    SignatureMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    CodeMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    NomMedecin = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitesSanitaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitesSanitaires_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalAudits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DateAction = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UtilisateurId = table.Column<int>(type: "INTEGER", nullable: true),
                    LoginUtilisateur = table.Column<string>(type: "TEXT", nullable: true),
                    TypeAction = table.Column<int>(type: "INTEGER", nullable: false),
                    EntiteAffectee = table.Column<string>(type: "TEXT", nullable: true),
                    IdEntiteAffectee = table.Column<int>(type: "INTEGER", nullable: true),
                    PatientId = table.Column<int>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    AnciennesValeurs = table.Column<string>(type: "TEXT", nullable: true),
                    NouvellesValeurs = table.Column<string>(type: "TEXT", nullable: true),
                    AdresseIP = table.Column<string>(type: "TEXT", nullable: true),
                    NomMachine = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalAudits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalAudits_Utilisateurs_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ContactsUrgence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EtatCivilId = table.Column<int>(type: "INTEGER", nullable: false),
                    NomComplet = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    LienParente = table.Column<string>(type: "TEXT", nullable: true),
                    Telephone = table.Column<string>(type: "TEXT", nullable: true),
                    TelephoneAlternatif = table.Column<string>(type: "TEXT", nullable: true),
                    Adresse = table.Column<string>(type: "TEXT", nullable: true),
                    EstPrioritaire = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactsUrgence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactsUrgence_EtatsCivils_EtatCivilId",
                        column: x => x.EtatCivilId,
                        principalTable: "EtatsCivils",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // ── Données initiales : administrateur par défaut ────────────
            migrationBuilder.InsertData(
                table: "Utilisateurs",
                columns: new[] { "Id", "Login", "MotDePasseHash", "Nom", "Prenom", "Role", "EstActif", "TentativesEchec", "CreatedAt", "IsDeleted" },
                values: new object[] { 1, "admin", "$2a$11$hFld1/jMKcREPGqVYYjde.RM5HWc9SDBIXRSLTWZbq0RBPdxhLOle", "Administrateur", "Système", 1, true, 0, new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc), false });

            // ── Index : unicité Login ─────────────────────────────────────
            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_Login",
                table: "Utilisateurs",
                column: "Login",
                unique: true);

            // ── Index : unicité NumeroCarnet ──────────────────────────────
            migrationBuilder.CreateIndex(
                name: "IX_Patients_NumeroCarnet",
                table: "Patients",
                column: "NumeroCarnet",
                unique: true);

            // ── Index FK : relations 1-1 (uniques) ───────────────────────
            migrationBuilder.CreateIndex(
                name: "IX_EtatsCivils_PatientId",
                table: "EtatsCivils",
                column: "PatientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExamensIncorporation_PatientId",
                table: "ExamensIncorporation",
                column: "PatientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControlesFinService_PatientId",
                table: "ControlesFinService",
                column: "PatientId",
                unique: true);

            // ── Index FK : relations 1-N ──────────────────────────────────
            migrationBuilder.CreateIndex(
                name: "IX_Constantes_PatientId",
                table: "Constantes",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationsMedicales_PatientId",
                table: "OperationsMedicales",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaccinations_PatientId",
                table: "Vaccinations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitesSanitaires_PatientId",
                table: "VisitesSanitaires",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Indisponibilites_PatientId",
                table: "Indisponibilites",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificatsMedicaux_PatientId",
                table: "CertificatsMedicaux",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DecisionsReforme_PatientId",
                table: "DecisionsReforme",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactsUrgence_EtatCivilId",
                table: "ContactsUrgence",
                column: "EtatCivilId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalAudits_UtilisateurId",
                table: "JournalAudits",
                column: "UtilisateurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ContactsUrgence");
            migrationBuilder.DropTable(name: "EtatsCivils");
            migrationBuilder.DropTable(name: "Constantes");
            migrationBuilder.DropTable(name: "ExamensIncorporation");
            migrationBuilder.DropTable(name: "OperationsMedicales");
            migrationBuilder.DropTable(name: "Vaccinations");
            migrationBuilder.DropTable(name: "VisitesSanitaires");
            migrationBuilder.DropTable(name: "Indisponibilites");
            migrationBuilder.DropTable(name: "CertificatsMedicaux");
            migrationBuilder.DropTable(name: "DecisionsReforme");
            migrationBuilder.DropTable(name: "ControlesFinService");
            migrationBuilder.DropTable(name: "JournalAudits");
            migrationBuilder.DropTable(name: "Patients");
            migrationBuilder.DropTable(name: "Utilisateurs");
        }
    }
}
