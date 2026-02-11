using CarnetSante.Core.Interfaces;
using CarnetSante.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CarnetSante.WPF.ViewModels;

public partial class PatientListViewModel : BaseViewModel
{
    private readonly IPatientRepository _repo;

    [ObservableProperty] private ObservableCollection<Patient> _patients = new();
    [ObservableProperty] private Patient? _selectedPatient;
    [ObservableProperty] private string _searchText = string.Empty;
    [ObservableProperty] private int _totalCount;

    public event Action<Patient>? OpenPatientRequested;
    public event Action? CreatePatientRequested;

    public PatientListViewModel(IPatientRepository repo)
    {
        _repo = repo;
    }

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            SetBusy("Chargement des patients...");
            var list = await _repo.GetAllAsync(SearchText);
            Patients = new ObservableCollection<Patient>(list);
            TotalCount = list.Count;
            SetIdle($"{list.Count} patient(s)");
        }
        catch (Exception ex)
        {
            SetError($"Erreur: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task SearchAsync()
    {
        await LoadAsync();
    }

    [RelayCommand]
    private void OpenPatient()
    {
        if (SelectedPatient != null)
            OpenPatientRequested?.Invoke(SelectedPatient);
    }

    [RelayCommand]
    private void CreatePatient()
    {
        CreatePatientRequested?.Invoke();
    }

    [RelayCommand]
    private async Task DeletePatientAsync()
    {
        if (SelectedPatient == null) return;
        try
        {
            SetBusy("Suppression...");
            await _repo.DeleteAsync(SelectedPatient.Id);
            await LoadAsync();
        }
        catch (Exception ex)
        {
            SetError($"Erreur: {ex.Message}");
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        _ = LoadAsync();
    }
}
