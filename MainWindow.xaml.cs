using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BindCurrency();
        }

        private void BindCurrency()
        {
            DataTable dtCurrency = CurrencyService.GetCurrencyTable();

            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbFromCurrency.DisplayMemberPath = "Text";
            cmbFromCurrency.SelectedValuePath = "Value";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            cmbToCurrency.DisplayMemberPath = "Text";
            cmbToCurrency.SelectedValuePath = "Value";
            cmbToCurrency.SelectedIndex = 0;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCurrency.Text))
            {
                MessageBox.Show("Please enter amount", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus();
                return;
            }

            if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please select currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }

            if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
            {
                MessageBox.Show("Please select currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbToCurrency.Focus();
                return;
            }

            if (!double.TryParse(txtCurrency.Text, NumberStyles.Number, CultureInfo.CurrentCulture, out double amount))
            {
                MessageBox.Show("Invalid amount", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                txtCurrency.Focus();
                return;
            }

            double fromRate = (double)cmbFromCurrency.SelectedValue;
            double toRate = (double)cmbToCurrency.SelectedValue;

            double converted = CurrencyService.ConvertAmount(amount, fromRate, toRate);

            lblCurrency.Content = $"{cmbToCurrency.Text} {converted:N3}";
        }

        private void Clear_Click(object sender, RoutedEventArgs e) => ClearControls();

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Allow digits and a single decimal separator (culture-aware)
            var decimalSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var tb = (System.Windows.Controls.TextBox)sender;

            if (e.Text == decimalSep)
            {
                // block if textbox already contains the decimal separator
                e.Handled = tb.Text.Contains(decimalSep);
                return;
            }   

            // allow control keys through (text composition usually supplies only the typed chars)
            e.Handled = !char.IsDigit(e.Text, 0);
        }

        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0) cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0) cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = string.Empty;
            txtCurrency.Focus();
        }
    }

    // Static helper moved out of the Window class so conversion logic and currency data are reusable/testable.
    public static class CurrencyService
    {
        public static DataTable GetCurrencyTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Value", typeof(double));

            dt.Rows.Add("--Select--", 0.0);
            dt.Rows.Add("INR", 1.0);
            dt.Rows.Add("USD", 75.0);
            dt.Rows.Add("EUR", 85.0);
            dt.Rows.Add("SAR", 20.0);
            dt.Rows.Add("POUND", 5.0);
            dt.Rows.Add("DEM", 43.0);

            return dt;
        }

        public static double ConvertAmount(double amount, double fromRate, double toRate)
        {
            // Protect division by zero; original logic multiplied fromRate * amount / toRate
            if (toRate == 0.0) return 0.0;
            return (fromRate * amount) / toRate;
        }
    }
}