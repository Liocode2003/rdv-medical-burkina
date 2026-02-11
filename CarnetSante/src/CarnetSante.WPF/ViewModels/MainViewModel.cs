using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CarnetSante.WPF.ViewModels;

public enum AppPage { Dashboard, Patients, PatientDetail }

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty] private AppPage _currentPage = AppPage.Dashboard;
    [ObservableProperty] private string _windowTitle = "CarnetSante - Ministère de la Santé du Burkina Faso";
    [ObservableProperty] private bool _isDashboardActive = true;
    [ObservableProperty] private bool _isPatientsActive;

    public DashboardViewModel DashboardVM { get; }
    public PatientListViewModel PatientListVM { get; }
    public PatientDetailViewModel PatientDetailVM { get; }

    public MainViewModel(
        DashboardViewModel dashboardVM,
        PatientListViewModel patientListVM,
        PatientDetailViewModel patientDetailVM)
    {
        DashboardVM = dashboardVM;
        PatientListVM = patientListVM;
        PatientDetailVM = patientDetailVM;

        PatientListVM.OpenPatientRequested += async (p) =>
        {
            await PatientDetailVM.LoadPatientAsync(p.Id);
            NavigateTo(AppPage.PatientDetail);
        };

        PatientListVM.CreatePatientRequested += async () =>
        {
            await PatientDetailVM.InitNewPatientAsync();
            NavigateTo(AppPage.PatientDetail);
        };

        PatientDetailVM.NavigateBackRequested += () =>
        {
            _ = PatientListVM.LoadAsync();
            NavigateTo(AppPage.Patients);
        };

        PatientDetailVM.SaveCompleted += () =>
        {
            // stay on detail page after save
        };
    }

    [RelayCommand]
    private void NavigateToDashboard()
    {
        NavigateTo(AppPage.Dashboard);
        _ = DashboardVM.LoadAsync();
    }

    [RelayCommand]
    private void NavigateToPatients()
    {
        NavigateTo(AppPage.Patients);
        _ = PatientListVM.LoadAsync();
    }

    private void NavigateTo(AppPage page)
    {
        CurrentPage = page;
        IsDashboardActive = page == AppPage.Dashboard;
        IsPatientsActive = page == AppPage.Patients || page == AppPage.PatientDetail;
    }
}
