using CommunityToolkit.Mvvm.ComponentModel;

namespace CarnetSante.WPF.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    protected void SetBusy(string message = "Chargement...")
    {
        IsBusy = true;
        StatusMessage = message;
        HasError = false;
        ErrorMessage = string.Empty;
    }

    protected void SetIdle(string message = "")
    {
        IsBusy = false;
        StatusMessage = message;
    }

    protected void SetError(string message)
    {
        IsBusy = false;
        HasError = true;
        ErrorMessage = message;
        StatusMessage = string.Empty;
    }
}
