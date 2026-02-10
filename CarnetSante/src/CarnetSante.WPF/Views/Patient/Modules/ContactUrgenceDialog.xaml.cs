using CarnetSante.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class ContactUrgenceDialog : Window
{
    public ContactUrgence? Contact { get; private set; }
    private readonly int _etatCivilId;

    public ContactUrgenceDialog(int etatCivilId)
    {
        InitializeComponent();
        _etatCivilId = etatCivilId;
    }

    private void BtnAjouter_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NomCompletBox.Text))
        {
            MessageBox.Show("Le nom complet est obligatoire.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        Contact = new ContactUrgence
        {
            EtatCivilId = _etatCivilId,
            NomComplet = NomCompletBox.Text.Trim(),
            LienParente = (LienParenteBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? LienParenteBox.Text,
            Telephone = TelephoneBox.Text.Trim(),
            TelephoneAlternatif = TelephoneAltBox.Text.Trim(),
            Adresse = AdresseBox.Text.Trim()
        };

        DialogResult = true;
        Close();
    }

    private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
