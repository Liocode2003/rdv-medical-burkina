-- ================================================================
-- SCRIPT SQL - CARNET DE SANTÉ SANITAIRE OFFICIEL
-- Burkina Faso - Schéma complet de la base de données SQLite
-- Version 1.0 - Génération manuelle pour référence
-- ================================================================

-- ── TABLE UTILISATEURS ──────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Utilisateurs (
    Id                  INTEGER PRIMARY KEY AUTOINCREMENT,
    Login               TEXT NOT NULL UNIQUE,
    MotDePasseHash      TEXT NOT NULL,
    Nom                 TEXT NOT NULL,
    Prenom              TEXT NOT NULL,
    Email               TEXT,
    Telephone           TEXT,
    Role                INTEGER NOT NULL DEFAULT 3,  -- 1=Admin, 2=Médecin, 3=Consultation
    EstActif            INTEGER NOT NULL DEFAULT 1,
    DerniereConnexion   TEXT,
    TentativesEchec     INTEGER NOT NULL DEFAULT 0,
    BloquéJusquau       TEXT,
    CodeMedecin         TEXT,
    Specialite          TEXT,
    CreatedAt           TEXT NOT NULL,
    UpdatedAt           TEXT,
    CreatedBy           TEXT,
    UpdatedBy           TEXT,
    IsDeleted           INTEGER NOT NULL DEFAULT 0
);

-- ── TABLE PATIENTS ───────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Patients (
    Id              INTEGER PRIMARY KEY AUTOINCREMENT,
    NumeroCarnet    TEXT NOT NULL UNIQUE,
    PhotoPath       TEXT,
    CreatedAt       TEXT NOT NULL,
    UpdatedAt       TEXT,
    CreatedBy       TEXT,
    UpdatedBy       TEXT,
    IsDeleted       INTEGER NOT NULL DEFAULT 0
);
CREATE INDEX idx_patients_numero ON Patients(NumeroCarnet);

-- ── MODULE A : ÉTATS CIVILS ──────────────────────────────────────
CREATE TABLE IF NOT EXISTS EtatsCivils (
    Id                    INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId             INTEGER NOT NULL UNIQUE,
    Nom                   TEXT NOT NULL,
    Prenoms               TEXT NOT NULL,
    DateNaissance         TEXT NOT NULL,
    LieuNaissance         TEXT NOT NULL,
    PaysNaissance         TEXT,
    Sexe                  INTEGER NOT NULL DEFAULT 1,
    GroupeSanguin         INTEGER NOT NULL DEFAULT 9,
    Nationalite           TEXT,
    NomPere               TEXT,
    PrenomsPere           TEXT,
    ProfessionPere        TEXT,
    NomMere               TEXT,
    PrenomsMere           TEXT,
    ProfessionMere        TEXT,
    SituationMatrimoniale TEXT,
    NombreEnfants         INTEGER,
    NumeroMatricule       TEXT,
    NumeroCNI             TEXT,
    NumeroPasseport       TEXT,
    Corps                 TEXT,
    Grade                 TEXT,
    Unite                 TEXT,
    Fonction              TEXT,
    DateRecrutement       TEXT,
    Adresse               TEXT,
    Ville                 TEXT,
    Telephone             TEXT,
    Email                 TEXT,
    EmpreintesNotes       TEXT,
    EmpreintesImagePath   TEXT,
    CreatedAt             TEXT NOT NULL,
    UpdatedAt             TEXT,
    CreatedBy             TEXT,
    UpdatedBy             TEXT,
    IsDeleted             INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);

-- ── CONTACTS D'URGENCE ───────────────────────────────────────────
CREATE TABLE IF NOT EXISTS ContactsUrgence (
    Id                  INTEGER PRIMARY KEY AUTOINCREMENT,
    EtatCivilId         INTEGER NOT NULL,
    NomComplet          TEXT NOT NULL,
    LienParente         TEXT,
    Telephone           TEXT,
    TelephoneAlternatif TEXT,
    Adresse             TEXT,
    EstPrioritaire      INTEGER NOT NULL DEFAULT 0,
    CreatedAt           TEXT NOT NULL,
    UpdatedAt           TEXT,
    CreatedBy           TEXT,
    UpdatedBy           TEXT,
    IsDeleted           INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (EtatCivilId) REFERENCES EtatsCivils(Id) ON DELETE CASCADE
);

-- ── MODULE B : CONSTANTES ────────────────────────────────────────
CREATE TABLE IF NOT EXISTS Constantes (
    Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId               INTEGER NOT NULL,
    DateMesure              TEXT NOT NULL,
    Taille                  REAL,
    Poids                   REAL,
    IMC                     REAL,
    PerimethreThoracique    REAL,
    PerimetreAbdominal      REAL,
    TensionSystolique       INTEGER,
    TensionDiastolique      INTEGER,
    FrequenceCardiaque      INTEGER,
    FrequenceRespiratoire   INTEGER,
    Temperature             REAL,
    Glycemie                REAL,
    UnitéGlycemie           TEXT DEFAULT 'g/L',
    Albumine                REAL,
    Spo2                    REAL,
    MedecinMesureur         TEXT,
    Observations            TEXT,
    CreatedAt               TEXT NOT NULL,
    UpdatedAt               TEXT,
    CreatedBy               TEXT,
    UpdatedBy               TEXT,
    IsDeleted               INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);
CREATE INDEX idx_constantes_patient_date ON Constantes(PatientId, DateMesure DESC);

-- ── MODULE C : EXAMENS D'INCORPORATION ──────────────────────────
CREATE TABLE IF NOT EXISTS ExamensIncorporation (
    Id                          INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId                   INTEGER NOT NULL UNIQUE,
    DateExamen                  TEXT NOT NULL,
    MedecinExaminateur          TEXT,
    AntecedentsHeredita         TEXT,
    AntecedentsPersonnels       TEXT,
    AntecedentsCollateraux      TEXT,
    AllergiesConnues            TEXT,
    TraitementsEnCours          TEXT,
    AppareilRespiratoire        TEXT,
    AppareilDigestif            TEXT,
    AppareilCirculatoire        TEXT,
    AppareilGenitourinaire      TEXT,
    SystemeNerveux              TEXT,
    SystemeOsteoarticulaire     TEXT,
    SystemeEndocrinien          TEXT,
    Denture                     TEXT,
    PeauAnnexes                 TEXT,
    VisionODSansCorrection      REAL,
    VisionOGSansCorrection      REAL,
    VisionODAvecCorrection      REAL,
    VisionOGAvecCorrection      REAL,
    TypeCorrectionOD            TEXT,
    TypeCorrectionOG            TEXT,
    EcartPupillaire             REAL,
    ObservationsVisuelles       TEXT,
    AuditionODVoixHaute         TEXT,
    AuditionOGVoixHaute         TEXT,
    AuditionODVoixChuchote      TEXT,
    AuditionOGVoixChuchote      TEXT,
    ObservationsAuditives       TEXT,
    SensChromatique             INTEGER,
    TypeDaltonisme              TEXT,
    ObservationsSensChromatique TEXT,
    AptitudeMedicale            INTEGER NOT NULL DEFAULT 1,
    MentionsMedicalesSpeciales  TEXT,
    RestrictionsActivite        TEXT,
    Observations                TEXT,
    Signature                   TEXT,
    CreatedAt                   TEXT NOT NULL,
    UpdatedAt                   TEXT,
    CreatedBy                   TEXT,
    UpdatedBy                   TEXT,
    IsDeleted                   INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);

-- ── MODULE D : OPÉRATIONS MÉDICALES ─────────────────────────────
CREATE TABLE IF NOT EXISTS OperationsMedicales (
    Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId               INTEGER NOT NULL,
    DateOperation           TEXT NOT NULL,
    Diagnostic              TEXT NOT NULL,
    TypeIntervention        INTEGER NOT NULL DEFAULT 2,
    DescriptionIntervention TEXT,
    LieuSejour              TEXT,
    DateAdmission           TEXT,
    DateSortie              TEXT,
    DureeSejour             INTEGER,
    EtatAvant               TEXT,
    EtatApres               TEXT,
    Complications           TEXT,
    SuitesDonnees           TEXT,
    ChirurgienPrincipal     TEXT,
    EquipeMedicale          TEXT,
    Observations            TEXT,
    SignatureMedecin        TEXT,
    CodeMedecin             TEXT,
    CompteRenduPath         TEXT,
    CreatedAt               TEXT NOT NULL,
    UpdatedAt               TEXT,
    CreatedBy               TEXT,
    UpdatedBy               TEXT,
    IsDeleted               INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);
CREATE INDEX idx_operations_patient ON OperationsMedicales(PatientId, DateOperation DESC);

-- ── MODULE E : VACCINATIONS ──────────────────────────────────────
CREATE TABLE IF NOT EXISTS Vaccinations (
    Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId               INTEGER NOT NULL,
    TypeVaccin              INTEGER NOT NULL DEFAULT 9,
    NomVaccin               TEXT NOT NULL,
    FabricantVaccin         TEXT,
    NumeroLot               TEXT,
    Reference               TEXT,
    DateVaccination         TEXT NOT NULL,
    NumeroDose              INTEGER NOT NULL DEFAULT 1,
    TypeDose                TEXT,
    QuantitéDose            REAL,
    VoieAdministration      TEXT,
    SiteInjection           TEXT,
    ProchaineInjectionPrevue TEXT,
    DateExpiration          TEXT,
    EstValide               INTEGER NOT NULL DEFAULT 1,
    ReactionPostVaccinale   TEXT,
    Observations            TEXT,
    MedecinVaccinateur      TEXT,
    CodeMedecin             TEXT,
    Etablissement           TEXT,
    CreatedAt               TEXT NOT NULL,
    UpdatedAt               TEXT,
    CreatedBy               TEXT,
    UpdatedBy               TEXT,
    IsDeleted               INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);
CREATE INDEX idx_vaccinations_patient ON Vaccinations(PatientId, TypeVaccin);

-- ── MODULE F : VISITES SANITAIRES ───────────────────────────────
CREATE TABLE IF NOT EXISTS VisitesSanitaires (
    Id                          INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId                   INTEGER NOT NULL,
    DateVisite                  TEXT NOT NULL,
    TypeVisite                  TEXT,
    EntiteMedicale              TEXT NOT NULL,
    ResultatsVisite             TEXT,
    DiagnosticsRetenus          TEXT,
    ExamensPrescrits            TEXT,
    TraitementsPrescrits        TEXT,
    AptitudeConclusionVisite    TEXT,
    ProchainContrôle            TEXT,
    Observations                TEXT,
    SignatureMedecin            TEXT,
    CodeMedecin                 TEXT,
    NomMedecin                  TEXT,
    CreatedAt                   TEXT NOT NULL,
    UpdatedAt                   TEXT,
    CreatedBy                   TEXT,
    UpdatedBy                   TEXT,
    IsDeleted                   INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);

-- ── MODULE G : INDISPONIBILITÉS ──────────────────────────────────
CREATE TABLE IF NOT EXISTS Indisponibilites (
    Id                  INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId           INTEGER NOT NULL,
    DateDebut           TEXT NOT NULL,
    DateFin             TEXT,
    DureePrescrite      INTEGER NOT NULL,
    DureeReelle         INTEGER,
    Statut              INTEGER NOT NULL DEFAULT 1,
    Motif               TEXT NOT NULL,
    Diagnostic          TEXT,
    CodeCIM10           TEXT,
    LieuSejour          TEXT,
    TypeSejour          TEXT,
    EtatDepart          TEXT,
    EtatRetour          TEXT,
    MedecinPrescripteur TEXT,
    Observations        TEXT,
    SignatureMedecin    TEXT,
    CreatedAt           TEXT NOT NULL,
    UpdatedAt           TEXT,
    CreatedBy           TEXT,
    UpdatedBy           TEXT,
    IsDeleted           INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);

-- ── MODULE H : CERTIFICATS MÉDICAUX ─────────────────────────────
CREATE TABLE IF NOT EXISTS CertificatsMedicaux (
    Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId               INTEGER NOT NULL,
    DateCertificat          TEXT NOT NULL,
    TypeCertificat          TEXT NOT NULL,
    Objet                   TEXT,
    Contenu                 TEXT,
    OrigineBlessureOuMaladie TEXT,
    ImputabiliteService     INTEGER,
    CirconstancesOrigine    TEXT,
    MedecinSignataire       TEXT,
    CodeMedecin             TEXT,
    Etablissement           TEXT,
    NumeroOrdre             TEXT,
    FichierPath             TEXT,
    FichierNom              TEXT,
    FichierTaille           INTEGER,
    FichierType             TEXT,
    Observations            TEXT,
    CreatedAt               TEXT NOT NULL,
    UpdatedAt               TEXT,
    CreatedBy               TEXT,
    UpdatedBy               TEXT,
    IsDeleted               INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);

-- ── MODULE I : DÉCISIONS DE RÉFORME ─────────────────────────────
CREATE TABLE IF NOT EXISTS DecisionsReforme (
    Id                      INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId               INTEGER NOT NULL,
    DateDecision            TEXT NOT NULL,
    NumeroDecision          TEXT NOT NULL,
    CompositionCommission   TEXT,
    LieuCommission          TEXT,
    DateReunionCommission   TEXT,
    Diagnostic              TEXT NOT NULL,
    CodeCIM10               TEXT,
    AffectionsPrincipales   TEXT,
    AffectionsAssociees     TEXT,
    TauxInvalidite          TEXT,
    Decision                INTEGER NOT NULL DEFAULT 1,
    MotifDecision           TEXT,
    ConditionsReengagement  TEXT,
    PensionAttribuee        INTEGER NOT NULL DEFAULT 0,
    TypePension             TEXT,
    TauxPension             REAL,
    Observations            TEXT NOT NULL,
    DateEffet               TEXT,
    CreatedAt               TEXT NOT NULL,
    UpdatedAt               TEXT,
    CreatedBy               TEXT,
    UpdatedBy               TEXT,
    IsDeleted               INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);

-- ── MODULE J : CONTRÔLE FIN DE SERVICE ──────────────────────────
CREATE TABLE IF NOT EXISTS ControlesFinService (
    Id                          INTEGER PRIMARY KEY AUTOINCREMENT,
    PatientId                   INTEGER NOT NULL UNIQUE,
    DateControle                TEXT NOT NULL,
    ExamenFinal                 TEXT,
    DiagnosticsFinaux           TEXT,
    EtatDeSante                 TEXT,
    AptitudeRejoindreForyer     INTEGER NOT NULL DEFAULT 1,
    ConditionsRejoindreForyer   TEXT,
    RecommandationsMedicales    TEXT,
    TraitementsDeLongDuree      TEXT,
    DateFinService              TEXT,
    DateRadiation               TEXT,
    MotifFinService             TEXT,
    NumeroDecisionRadiation     TEXT,
    RecapitulatifPathologies    TEXT,
    RecapitulatifInterventions  TEXT,
    RecapitulatifIndisponibilites TEXT,
    TotalJoursIndisponibilite   INTEGER,
    MedecinSignataire           TEXT,
    CodeMedecin                 TEXT,
    SignatureMedecin            TEXT,
    Observations                TEXT,
    CreatedAt                   TEXT NOT NULL,
    UpdatedAt                   TEXT,
    CreatedBy                   TEXT,
    UpdatedBy                   TEXT,
    IsDeleted                   INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (PatientId) REFERENCES Patients(Id) ON DELETE CASCADE
);

-- ── JOURNAL D'AUDIT ──────────────────────────────────────────────
CREATE TABLE IF NOT EXISTS JournalAudits (
    Id                  INTEGER PRIMARY KEY AUTOINCREMENT,
    DateAction          TEXT NOT NULL,
    UtilisateurId       INTEGER,
    LoginUtilisateur    TEXT,
    TypeAction          INTEGER NOT NULL,
    EntiteAffectee      TEXT,
    IdEntiteAffectee    INTEGER,
    PatientId           INTEGER,
    Description         TEXT,
    AnciennesValeurs    TEXT,
    NouvellesValeurs    TEXT,
    AdresseIP           TEXT,
    NomMachine          TEXT,
    FOREIGN KEY (UtilisateurId) REFERENCES Utilisateurs(Id) ON DELETE SET NULL
);
CREATE INDEX idx_journal_date ON JournalAudits(DateAction DESC);
CREATE INDEX idx_journal_patient ON JournalAudits(PatientId);

-- ── DONNÉES INITIALES ────────────────────────────────────────────
INSERT OR IGNORE INTO Utilisateurs (
    Login, MotDePasseHash, Nom, Prenom, Role, EstActif, CreatedAt
) VALUES (
    'admin',
    '$2a$12$abc...', -- À remplacer par un vrai hash BCrypt de votre mot de passe
    'Administrateur',
    'Système',
    1,
    1,
    datetime('now')
);

-- ================================================================
-- FIN DU SCRIPT
-- ================================================================
