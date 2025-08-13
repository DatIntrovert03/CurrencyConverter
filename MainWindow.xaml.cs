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
using System.Configuration;
using Microsoft.Data.SqlClient;//we need to install the microsoft.Data.Sqlclient Nuget Package


namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        private int CurrencyId = 0;
        private double FromAmount = 0;
        private double ToAmount = 0;
        public MainWindow(){
            InitializeComponent();
            BindCurrency();
            mycon();
        }
           private void BindCurrency(){
            mycon();
            DataTable dtCurrency = new DataTable();
            string query = "Select Id,CurrencyName from Currency_Master";
            cmd = new SqlCommand(query,con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dtCurrency);
            cmd.ExecuteScalar();

            DataRow newRow = dtCurrency.NewRow();
            newRow["Id"] = 0;
            newRow["CurrencyName"] = "--Select--";
            dtCurrency.Rows.InsertAt(newRow,0);
            if(dtCurrency != null && dtCurrency.Rows.Count>0)
            {
                cmbFromCurrency.ItemsSource = dtCurrency.DefaultView;
                cmbToCurrency.ItemsSource = dtCurrency.DefaultView;
            }

            con.Close();

            cmbFromCurrency.DisplayMemberPath = "CurrencyName";
            cmbFromCurrency.SelectedValuePath = "Id";
            cmbFromCurrency.SelectedIndex = 0;

                    cmbToCurrency.DisplayMemberPath = "CurrencyName";
                    cmbToCurrency.SelectedValuePath = "Id";
                    cmbToCurrency.SelectedIndex = 0;
                            
        }
        private void mycon()
        {
            string Conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString();
            con = new SqlConnection();
            con.Open();
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