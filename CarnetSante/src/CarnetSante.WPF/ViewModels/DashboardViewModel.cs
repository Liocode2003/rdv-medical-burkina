using CarnetSante.Core.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarnetSante.WPF.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly IPatientRepository _patientRepo;

    [ObservableProperty] private int _totalPatients;
    [ObservableProperty] private int _patientsHommes;
    [ObservableProperty] private int _patientsFemmes;
    [ObservableProperty] private string _dateAujourdhui = DateTime.Now.ToString("dddd dd MMMM yyyy",
        System.Globalization.CultureInfo.GetCultureInfo("fr-FR"));

    public DashboardViewModel(IPatientRepository patientRepo)
    {
        _patientRepo = patientRepo;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            SetBusy("Chargement des statistiques...");
            var patients = await _patientRepo.GetAllAsync();
            TotalPatients = patients.Count;
            PatientsHommes = patients.Count(p => p.Sexe == Core.Enums.Sexe.Masculin);
            PatientsFemmes = patients.Count(p => p.Sexe == Core.Enums.Sexe.Feminin);
            SetIdle();
        }
        catch (Exception ex)
        {
            SetError($"Erreur: {ex.Message}");
        }
    }
}
