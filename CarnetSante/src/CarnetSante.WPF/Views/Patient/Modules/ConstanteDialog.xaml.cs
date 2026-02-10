using CarnetSante.Core.Models;
using System.Windows;

namespace CarnetSante.WPF.Views.Patient.Modules;

public partial class ConstanteDialog : Window
{
    public Constante? Constante { get; private set; }
    private readonly int _patientId;

    public ConstanteDialog(int patientId)
    {
        InitializeComponent();
        _patientId = patientId;
        DateMesurePicker.SelectedDate = DateTime.Today;

        // Calcul IMC automatique
        TailleBox.TextChanged += (_, _) => CalculerImc();
        PoidsBox.TextChanged  += (_, _) => CalculerImc();
    }

    private void CalculerImc()
    {
        if (decimal.TryParse(TailleBox.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var taille) &&
            decimal.TryParse(PoidsBox.Text.Replace(',', '.'), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var poids) &&
            taille > 0)
        {
            var tailleM = taille / 100m;
            ImcBox.Text = Math.Round(poids / (tailleM * tailleM), 1).ToString("F1");
        }
        else
        {
            ImcBox.Text = string.Empty;
        }
    }

    private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
    {
        if (DateMesurePicker.SelectedDate == null)
        {
            MessageBox.Show("La date de mesure est obligatoire.", "Validation",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        decimal? Parse(string text) =>
            decimal.TryParse(text.Replace(',', '.'), System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : null;

        int? ParseInt(string text) =>
            int.TryParse(text, out var v) ? v : null;

        Constante = new Constante
        {
            PatientId      = _patientId,
            DateMesure     = DateMesurePicker.SelectedDate.Value,
            MedecinMesureur = MedecinBox.Text.Trim(),
            Taille         = Parse(TailleBox.Text),
            Poids          = Parse(PoidsBox.Text),
            IMC            = Parse(ImcBox.Text),
            TensionSystolique  = ParseInt(TASystoBox.Text),
            TensionDiastolique = ParseInt(TADiastoBox.Text),
            FrequenceCardiaque = ParseInt(FcBox.Text),
            FrequenceRespiratoire = ParseInt(FrBox.Text),
            Temperature    = Parse(TempBox.Text),
            Spo2           = Parse(Spo2Box.Text),
            Glycemie       = Parse(GlycemieBox.Text),
            Albumine       = Parse(AlbumineBox.Text),
            Observations   = ObservationsBox.Text.Trim()
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
