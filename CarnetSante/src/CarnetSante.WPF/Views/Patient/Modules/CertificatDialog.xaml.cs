using CarnetSante.Core.Models;
using System.Windows;
using System.Windows.Controls;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class CertificatDialog : Window
{
    public CertificatMedical? Certificat { get; private set; }
    private readonly int _patientId;

    public CertificatDialog(int patientId)
    {
        InitializeComponent();
        _patientId = patientId;
        DateCertificatPicker.SelectedDate = DateTime.Today;
    }

    private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (DateCertificatPicker.SelectedDate == null)
        {
            MessageBox.Show("La date du certificat est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var typeCertificat = TypeCertificatBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(typeCertificat))
        {
            MessageBox.Show("Le type de certificat est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Récupère le texte de la ComboBox (éditable)
        if (TypeCertificatBox.SelectedItem is ComboBoxItem item)
            typeCertificat = item.Content?.ToString() ?? typeCertificat;

        Certificat = new CertificatMedical
        {
            PatientId               = _patientId,
            DateCertificat          = DateCertificatPicker.SelectedDate.Value,
            TypeCertificat          = typeCertificat,
            Objet                   = ObjetBox.Text.Trim(),
            Contenu                 = ContenuBox.Text.Trim(),
            OrigineBlessureOuMaladie = OrigineBox.Text.Trim(),
            CirconstancesOrigine    = CirconstancesBox.Text.Trim(),
            ImputabiliteService     = ImputabiliteBox.IsChecked,
            MedecinSignataire       = MedecinBox.Text.Trim(),
            NumeroOrdre             = NumeroOrdreBox.Text.Trim(),
            Etablissement           = EtablissementBox.Text.Trim(),
            Observations            = ObservationsBox.Text.Trim()
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
