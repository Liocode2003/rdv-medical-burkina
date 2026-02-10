using CarnetSante.WPF.ViewModels.Patient;
using System.Windows;

namespace CarnetSante.WPF.Views.Patient;

public partial class PatientWindow : Window
{
    public PatientViewModel ViewModel { get; }

    public PatientWindow(PatientViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = ViewModel;

        // Abonnement aux événements du ViewModel
        ViewModel.DemanderNouvelleConstante      += OnDemanderNouvelleConstante;
        ViewModel.DemanderNouvelleVaccination    += OnDemanderNouvelleVaccination;
        ViewModel.DemanderNouvelleVisite         += OnDemanderNouvelleVisite;
        ViewModel.DemanderNouvelleIndisponibilite += OnDemanderNouvelleIndisponibilite;
        ViewModel.DemanderNouveauCertificat      += OnDemanderNouveauCertificat;
        ViewModel.DemanderNouvelleDecisionReforme += OnDemanderNouvelleDecisionReforme;
    }

    private void BtnFermer_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.EstModifie)
        {
            var result = MessageBox.Show(
                "Des modifications non sauvegardées seront perdues. Voulez-vous quand même fermer ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.No) return;
        }
        Close();
    }

    // ── Module A : Contact d'urgence ─────────────────────────────
    private void BtnAjouterContact_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Modules.ContactUrgenceDialog(ViewModel.Patient?.EtatCivil?.Id ?? 0);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Contact != null)
        {
            ViewModel.Patient?.EtatCivil?.ContactsUrgence.Add(dialog.Contact);
            ViewModel.MarquerModifie();
        }
    }

    // ── Module B : Constantes ─────────────────────────────────────
    private void OnDemanderNouvelleConstante()
    {
        if (ViewModel.Patient == null) return;
        var dialog = new Modules.ConstanteDialog(ViewModel.Patient.Id);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Constante != null)
        {
            ViewModel.Patient.Constantes.Add(dialog.Constante);
            ViewModel.MarquerModifie();
        }
    }

    // ── Module D : Opérations (bouton dans le XAML) ──────────────
    private void BtnNouvelleOperation_Click(object sender, RoutedEventArgs e)
    {
        if (ViewModel.Patient == null) return;
        var dialog = new Modules.OperationDialog(ViewModel.Patient.Id);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Operation != null)
        {
            ViewModel.Patient.OperationsMedicales.Add(dialog.Operation); // corrigé : Add() pas Append()
            ViewModel.MarquerModifie();
        }
    }

    // ── Module E : Vaccinations ───────────────────────────────────
    private void OnDemanderNouvelleVaccination()
    {
        if (ViewModel.Patient == null) return;
        var dialog = new Modules.VaccinationDialog(ViewModel.Patient.Id);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Vaccination != null)
        {
            ViewModel.Patient.Vaccinations.Add(dialog.Vaccination);
            ViewModel.MarquerModifie();
        }
    }

    // ── Module F : Visites sanitaires ────────────────────────────
    private void OnDemanderNouvelleVisite()
    {
        if (ViewModel.Patient == null) return;
        var dialog = new Modules.VisiteDialog(ViewModel.Patient.Id);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Visite != null)
        {
            ViewModel.Patient.VisitesSanitaires.Add(dialog.Visite);
            ViewModel.MarquerModifie();
        }
    }

    // ── Module G : Indisponibilités ───────────────────────────────
    private void OnDemanderNouvelleIndisponibilite()
    {
        if (ViewModel.Patient == null) return;
        var dialog = new Modules.IndisponibiliteDialog(ViewModel.Patient.Id);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Indisponibilite != null)
        {
            ViewModel.Patient.Indisponibilites.Add(dialog.Indisponibilite);
            ViewModel.MarquerModifie();
        }
    }

    // ── Module H : Certificats médicaux ──────────────────────────
    private void OnDemanderNouveauCertificat()
    {
        if (ViewModel.Patient == null) return;
        var dialog = new Modules.CertificatDialog(ViewModel.Patient.Id);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Certificat != null)
        {
            ViewModel.Patient.CertificatsMedicaux.Add(dialog.Certificat);
            ViewModel.MarquerModifie();
        }
    }

    // ── Module I : Décisions de réforme ──────────────────────────
    private void OnDemanderNouvelleDecisionReforme()
    {
        if (ViewModel.Patient == null) return;
        var dialog = new Modules.DecisionReformeDialog(ViewModel.Patient.Id);
        dialog.Owner = this;
        if (dialog.ShowDialog() == true && dialog.Decision != null)
        {
            ViewModel.Patient.DecisionsReforme.Add(dialog.Decision);
            ViewModel.MarquerModifie();
        }
    }
}
