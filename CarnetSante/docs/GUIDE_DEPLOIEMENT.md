# Guide de Déploiement - CarnetSanté Burkina Faso

## 1. PRÉREQUIS SYSTÈME

| Composant | Version minimale |
|-----------|-----------------|
| Windows   | Windows 10 / 11 64-bit |
| .NET Runtime | .NET 8.0 Desktop Runtime |
| RAM | 4 Go minimum |
| Disque | 500 Mo (+ espace données) |

---

## 2. INSTALLATION DU RUNTIME .NET 8

Télécharger depuis : https://dotnet.microsoft.com/download/dotnet/8.0

Choisir : **.NET Desktop Runtime 8.0.x** (Windows x64)

---

## 3. COMPILATION DU PROJET

### Depuis Visual Studio 2022+
1. Ouvrir `CarnetSante.sln`
2. Menu Build → Publish...
3. Target : `win-x64`, SelfContained: `true`
4. Cliquer **Publish**

### Depuis la ligne de commande
```bash
cd CarnetSante/src/CarnetSante.WPF
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

---

## 4. DÉPLOIEMENT SUR POSTE MÉDICAL

1. Copier le fichier `CarnetSante.exe` (et DLLs) dans `C:\Programs\CarnetSante\`
2. Créer un raccourci sur le Bureau
3. Lancer l'application UNE PREMIÈRE FOIS pour créer la base de données

La base de données SQLite est créée automatiquement dans :
```
%LOCALAPPDATA%\CarnetSante\carnet_sante.db
```

---

## 5. PREMIÈRE CONNEXION

| Champ | Valeur |
|-------|--------|
| Identifiant | `admin` |
| Mot de passe | `Admin@2024!` |

> ⚠️ **IMPÉRATIF** : Changer le mot de passe administrateur immédiatement.

---

## 6. CRÉATION DES COMPTES UTILISATEURS

1. Se connecter en tant qu'Administrateur
2. Menu Administration → Utilisateurs → Nouvel Utilisateur
3. Remplir : Login, Nom, Prénom, Rôle, Mot de passe
4. Rôles disponibles :
   - **Administrateur** : Accès complet + gestion utilisateurs
   - **Médecin** : Lecture/écriture dossiers médicaux
   - **Consultation** : Lecture seule

---

## 7. SAUVEGARDE DE LA BASE DE DONNÉES

### Sauvegarde manuelle
1. Quitter l'application
2. Copier le fichier `%LOCALAPPDATA%\CarnetSante\carnet_sante.db` vers un support externe

### Sauvegarde depuis l'application
Menu Administration → Sauvegarde → Créer une sauvegarde

---

## 8. RESTAURATION

1. Quitter l'application
2. Remplacer `carnet_sante.db` par la sauvegarde
3. Relancer l'application

---

## 9. STRUCTURE DES FICHIERS

```
CarnetSante.exe              ← Exécutable principal
appsettings.json             ← Configuration
%LOCALAPPDATA%\CarnetSante\
  ├── carnet_sante.db        ← Base de données SQLite
  ├── backups\               ← Sauvegardes automatiques
  └── exports\               ← PDF exportés
```

---

## 10. RÉSOLUTION DE PROBLÈMES

### L'application ne démarre pas
- Vérifier que .NET 8 Desktop Runtime est installé
- Vérifier les droits d'écriture dans `%LOCALAPPDATA%`

### Erreur "Base de données corrompue"
- Restaurer depuis la dernière sauvegarde

### Mot de passe oublié (Administrateur)
Contacter le support technique pour réinitialisation.

---

## 11. SÉCURITÉ ET CONFIDENTIALITÉ

- **Données médicales confidentielles** : Accès restreint au personnel habilité
- Toutes les actions sont enregistrées dans le journal d'audit
- Les mots de passe sont hachés avec BCrypt (workFactor=12)
- Verrouillage automatique après 5 tentatives échouées

---

## 12. SUPPORT

Pour tout problème technique, contacter l'administrateur système ou le service informatique du Ministère de la Santé.
