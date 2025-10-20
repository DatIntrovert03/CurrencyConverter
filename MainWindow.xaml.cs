using Newtonsoft.Json;
using System;
using System.Data;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        Root val = new Root();
        public MainWindow()
        {
            InitializeComponent();
            GetValue();
            
        }
        public static async Task<Root> GetData<T> (string url)
        {
            var myroot = new Root();
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(1);
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        var responseObject = JsonConvert.DeserializeObject<Root>(responseString);
                        //MessageBox.Show("TimeStamp: " + responseObject.timestamp, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        return responseObject;
                    }
                    return myroot;
                }
            }
            catch{
                return myroot;
            }
        }
        public async void GetValue()
        {
            val = await GetData<Root>("https://openexchangerates.org/api/latest.json?app_id=c6c38010df72452eae0eeda04f3e6f2c");
            BindCurrency();
        }
        private void BindCurrency()
        {
            DataTable dtCurrency = BuildCurrencyTable();

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

            double converted = ConvertAmount(amount, fromRate, toRate);

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

        // Integrated (non-reusable) currency helpers moved inside the Window class:

        private DataTable BuildCurrencyTable()
        {
            var dt = new DataTable();
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Value", typeof(double));

            // Placeholder row uses 0.0 as the value so it's not a valid rate
            dt.Rows.Add("--Select--");
            dt.Rows.Add("INR",val.rates.INR);
            dt.Rows.Add("USD", val.rates.USD);
            dt.Rows.Add("EUR", val.rates.EUR);
            dt.Rows.Add("CHF", val.rates.CHF);
            dt.Rows.Add("POUND", val.rates.GBP);
            dt.Rows.Add("AUD", val.rates.AUD);
            dt.Rows.Add("CAD", val.rates.CAD);
            dt.Rows.Add("CNY", val.rates.CNY);
            dt.Rows.Add("JPY", val.rates.JPY);
            return dt;
        }

        private double ConvertAmount(double amount, double fromRate, double toRate)
        {
            // Protect division by zero
            if (toRate == 0.0) return 0.0;
            return (toRate * amount) / fromRate;
        }
    }
}