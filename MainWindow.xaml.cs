using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(){
            InitializeComponent();
            BindCurrency();
        }
           private void BindCurrency(){
            DataTable dtCurrency = new DataTable();
            dtCurrency.Columns.Add("Text");
            dtCurrency.Columns.Add("Value");
            dtCurrency.Rows.Add("--Select--",0);
            dtCurrency.Rows.Add("INR", 1);
            dtCurrency.Rows.Add("USD", 75);
            dtCurrency.Rows.Add("EUR", 85);
            dtCurrency.Rows.Add("SAR", 20);
            dtCurrency.Rows.Add("POUND",5);
            dtCurrency.Rows.Add("DEM", 43);

                            cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;
                            cmbFromCurrency.DisplayMemberPath = "Text";
                            cmbFromCurrency.SelectedValuePath = "Value";
                            cmbFromCurrency.SelectedIndex = 0;

                                cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
                                cmbToCurrency.DisplayMemberPath = "Text";
                                cmbToCurrency.SelectedValuePath = "Value";
                                cmbToCurrency.SelectedIndex = 0;
        }
        private void Convert_Click(object sender, RoutedEventArgs e){
            double ConvertedValue;
            if (txtCurrency.Text==null||txtCurrency.Text.Trim()==""){
                MessageBox.Show("Please Enter Currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                txtCurrency.Focus();
                return;
            }
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0){
                MessageBox.Show("Please Select Currency From", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbFromCurrency.Focus();
                return;
            }
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0){
                MessageBox.Show("Please Select Currency To", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                cmbToCurrency.Focus();
                return;
            }
            //If From and To Combobox selected values are same
            if (cmbFromCurrency.Text == cmbToCurrency.Text){
                ConvertedValue = double.Parse(txtCurrency.Text);
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else{
                ConvertedValue = (double.Parse(cmbFromCurrency.SelectedValue.ToString()) * double.Parse(txtCurrency.Text)) / double.Parse(cmbToCurrency.SelectedValue.ToString());
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e){
            ClearControls();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e){
            
        }
        private void ClearControls()
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
                cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void dgvCurrency_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}