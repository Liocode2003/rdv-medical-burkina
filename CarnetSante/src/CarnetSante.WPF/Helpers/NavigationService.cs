using System.Windows;

namespace CarnetSante.WPF.Helpers;

/// <summary>
/// Service de navigation entre les fenêtres WPF.
/// </summary>
public interface INavigationService
{
    void AfficherMessage(string titre, string message, bool estErreur = false);
    bool DemanderConfirmation(string titre, string question);
    string? ChoisirFichier(string filtre, string titre);
    string? SauvegarderFichier(string nomDefaut, string filtre);
}

public class NavigationService : INavigationService
{
    public void AfficherMessage(string titre, string message, bool estErreur = false)
    {
        var icon = estErreur ? MessageBoxImage.Error : MessageBoxImage.Information;
        MessageBox.Show(message, titre, MessageBoxButton.OK, icon);
    }

    public bool DemanderConfirmation(string titre, string question)
    {
        var result = MessageBox.Show(question, titre,
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }

    public string? ChoisirFichier(string filtre, string titre)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog
        {
            Title = titre,
            Filter = filtre
        };
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }

    public string? SauvegarderFichier(string nomDefaut, string filtre)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            FileName = nomDefaut,
            Filter = filtre
        };
        return dialog.ShowDialog() == true ? dialog.FileName : null;
    }
}
