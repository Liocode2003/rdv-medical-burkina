using CarnetSante.Core.Models;
using CarnetSante.Core.Services;
using CarnetSante.WPF.ViewModels.Admin;
using CarnetSante.WPF.ViewModels.Dashboard;
using CarnetSante.WPF.Views.Admin;
using CarnetSante.WPF.Views.Patient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarnetSante.WPF.Views.Dashboard;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;
    private readonly Func<PatientWindow> _patientWindowFactory;
    private readonly Func<GestionUtilisateursWindow> _utilisateursWindowFactory;
    private readonly Func<JournalAuditWindow> _journalAuditWindowFactory;
    private readonly IAuthService _authService;

    public MainWindow(
        MainViewModel viewModel,
        Func<PatientWindow> patientWindowFactory,
        Func<GestionUtilisateursWindow> utilisateursWindowFactory,
        Func<JournalAuditWindow> journalAuditWindowFactory,
        IAuthService authService)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _patientWindowFactory          = patientWindowFactory;
        _utilisateursWindowFactory     = utilisateursWindowFactory;
        _journalAuditWindowFactory     = journalAuditWindowFactory;
        _authService                   = authService;
        DataContext = _viewModel;

        _viewModel.OuvrirFichePatient         += OnOuvrirFichePatient;
        _viewModel.DemanderDeconnexion        += OnDemanderDeconnexion;
        _viewModel.DemanderOuvrirUtilisateurs += OnOuvrirUtilisateurs;
        _viewModel.DemanderOuvrirJournalAudit += OnOuvrirJournalAudit;
        _viewModel.DemanderChangerMdp         += OnChangerMdp;
    }

    private void OnOuvrirFichePatient(Core.Models.Patient? patient)
    {
        var window = _patientWindowFactory();
        if (patient != null)
            _ = window.ViewModel.ChargerPatientAsync(patient.Id);
        else
            _ = window.ViewModel.InitialiserNouveauPatientAsync();

        window.Owner = this;
        window.ShowDialog();
        _ = _viewModel.ChargerPatientsCommand.Execute(null);
    }

    private void OnDemanderDeconnexion()
    {
        var loginWindow = App.GetLoginWindow();
        loginWindow.Show();
        Close();
    }

    private void OnOuvrirUtilisateurs()
    {
        var window = _utilisateursWindowFactory();
        window.Owner = this;
        window.ShowDialog();
    }

    private void OnOuvrirJournalAudit()
    {
        var window = _journalAuditWindowFactory();
        window.Owner = this;
        window.ShowDialog();
    }

    private void OnChangerMdp()
    {
        var dialog = new ChangerMotDePasseDialog(_authService);
        dialog.Owner = this;
        dialog.ShowDialog();
    }

    private void PatientListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (_viewModel.PatientSelectionne != null)
            _viewModel.OuvrirPatient(_viewModel.PatientSelectionne);
    }

    private void BtnOuvrir_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Core.Models.Patient patient)
            _viewModel.OuvrirPatient(patient);
    }
}
