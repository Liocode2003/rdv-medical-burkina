# Architecture Technique - CarnetSanté

## DIAGRAMME D'ARCHITECTURE

```
┌─────────────────────────────────────────────────────────────┐
│                    COUCHE PRÉSENTATION                       │
│  CarnetSante.WPF (.NET 8 / WPF / MVVM)                     │
│                                                             │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────────┐  │
│  │  Views   │  │  ViewModels  │  │      Services WPF     │  │
│  │ (XAML)   │←→│  (Binding)   │←→│  PdfService           │  │
│  │ A→J Tabs │  │ MainViewModel│  │                       │  │
│  └──────────┘  └──────────────┘  └──────────────────────┘  │
└─────────────────────────────┬───────────────────────────────┘
                              │ Interfaces (IoC)
┌─────────────────────────────▼───────────────────────────────┐
│                    COUCHE MÉTIER                             │
│  CarnetSante.Core (.NET 8 Standard)                         │
│                                                             │
│  ┌──────────┐  ┌──────────────┐  ┌──────────────────────┐  │
│  │  Models  │  │   Services   │  │       Enums           │  │
│  │ Patient  │  │  IAuthService│  │  UserRole             │  │
│  │ EtatCivil│  │  IAuditSvc   │  │  AptitudeMedicale     │  │
│  │ Constante│  │  IPatientSvc │  │  TypeVaccin           │  │
│  │ Vaccin.. │  │  IPdfService │  │  TypeIntervention     │  │
│  └──────────┘  └──────────────┘  └──────────────────────┘  │
└─────────────────────────────┬───────────────────────────────┘
                              │ Dépendances
┌─────────────────────────────▼───────────────────────────────┐
│                    COUCHE DONNÉES                            │
│  CarnetSante.Data (.NET 8)                                  │
│                                                             │
│  ┌─────────────────┐  ┌──────────────────────────────────┐  │
│  │  DbContext      │  │  Repositories                    │  │
│  │  EF Core 8.0    │  │  PatientRepository               │  │
│  │  SQLite         │  │  UtilisateurRepository           │  │
│  │                 │  │  + AuthService, AuditService      │  │
│  └─────────────────┘  │  + PatientService                │  │
│                       └──────────────────────────────────┘  │
└─────────────────────────────┬───────────────────────────────┘
                              │
┌─────────────────────────────▼───────────────────────────────┐
│                    BASE DE DONNÉES                           │
│  SQLite (carnet_sante.db)                                   │
│                                                             │
│  Utilisateurs │ Patients │ EtatsCivils │ ContactsUrgence   │
│  Constantes │ ExamensIncorporation │ OperationsMedicales    │
│  Vaccinations │ VisitesSanitaires │ Indisponibilites        │
│  CertificatsMedicaux │ DecisionsReforme │ ControlesFinService│
│  JournalAudits                                              │
└─────────────────────────────────────────────────────────────┘
```

## STRUCTURE DES MODULES (A→J)

```
Module A - État Civil
  └── Patient.EtatCivil (1-1)
  └── EtatCivil.ContactsUrgence (1-N)

Module B - Constantes
  └── Patient.Constantes (1-N) [chronologique]

Module C - Examen Incorporation
  └── Patient.ExamenIncorporation (1-1)

Module D - Opérations Médicales
  └── Patient.OperationsMedicales (1-N)

Module E - Vaccinations
  └── Patient.Vaccinations (1-N)

Module F - Visites Sanitaires
  └── Patient.VisitesSanitaires (1-N)

Module G - Indisponibilités
  └── Patient.Indisponibilites (1-N)

Module H - Certificats Médicaux
  └── Patient.CertificatsMedicaux (1-N)

Module I - Décisions de Réforme
  └── Patient.DecisionsReforme (1-N)

Module J - Contrôle Fin de Service
  └── Patient.ControleFinService (1-1)
```

## SÉCURITÉ

| Mécanisme | Implémentation |
|-----------|----------------|
| Hachage MDP | BCrypt.Net workFactor=12 |
| Verrouillage | 5 tentatives → 30 min blocage |
| Rôles | 3 niveaux (Admin/Médecin/Consultation) |
| Audit | Journal immuable de toutes les actions |
| Soft-delete | Données jamais supprimées physiquement |

## FLUX D'AUTHENTIFICATION

```
Utilisateur → LoginWindow → LoginViewModel → IAuthService
                                                    ↓
                                           UtilisateurRepository
                                                    ↓
                                           BCrypt.Verify(mdp)
                                                    ↓
                                  [OK] → Session ouverte
                                  [KO] → Incrémenter tentatives
```
