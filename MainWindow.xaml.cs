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
            GetData();
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
            con = new SqlConnection(Conn);
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
            try
            {
                if (txtAmount.Text == null || txtAmount.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Amount", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtAmount.Focus();
                    return;
                }
                else if (txtCurrencyName.Text == null || txtCurrencyName.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Currency Name", "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                    txtCurrencyName.Focus();
                    return;

                }
                else
                {
                    if (CurrencyId > 0)
                    {
                        if (MessageBox.Show("Are you sure you want to Update ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            mycon();
                            DataTable dt = new DataTable();
                            cmd = new SqlCommand("UPDATE Currency_Master SET Amount=@Amount,CurrencyName=@CurrencyName Where Id= @Id", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Id", CurrencyId);
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Data Updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            GetData();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Are you sure you want to Save ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            mycon();
                            DataTable dt = new DataTable();
                            cmd = new SqlCommand("INSERT INTO Currency_Master(Amount, CurrencyName) VALUES(@Amount, @CurrencyName)", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                            cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Data saved successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            GetData();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
      private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void dgvCurrency_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                
                DataGrid grd = (DataGrid)sender;
                DataRowView row_selected = grd.CurrentItem as DataRowView;
                if (row_selected != null)
                {

                  
                    if (dgvCurrency.Items.Count > 0)
                    {
                        if (grd.SelectedCells.Count > 0)
                        {
                            CurrencyId = Int32.Parse(row_selected["Id"].ToString());
                            if (grd.SelectedCells[0].Column.DisplayIndex == 0)
                            {
                                txtAmount.Text = row_selected["Amount"].ToString();
                                txtCurrencyName.Text = row_selected["CurrencyName"].ToString();
                                btnSave.Content = "Update";
                            }     
                            if (grd.SelectedCells[0].Column.DisplayIndex == 1)
                            {
                                if (MessageBox.Show("Are you sure you want to delete ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    mycon();
                                    DataTable dt = new DataTable();
                                    cmd = new SqlCommand("DELETE FROM Currency_Master WHERE Id = @Id", con);
                                    cmd.CommandType = CommandType.Text;
                                    cmd.Parameters.AddWithValue("@Id", CurrencyId);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    MessageBox.Show("Data deleted successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        public void GetData()
        {
            mycon();
            DataTable dt = new DataTable();    
            cmd = new SqlCommand("SELECT * FROM Currency_Master", con);    
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
  
            da.Fill(dt);
            if (dt != null && dt.Rows.Count > 0)
            {
                dgvCurrency.ItemsSource = dt.DefaultView;
            }
            else
            {
                dgvCurrency.ItemsSource = null;
            }
            con.Close();
        }
        public void ClearMaster()
        {
            try
            {
                txtAmount.Text = string.Empty;
                txtCurrencyName.Text = string.Empty;
                btnSave.Content = "Save";
                GetData();
                BindCurrency();
                txtAmount.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
         

        }
    }
}