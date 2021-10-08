using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Temp_UI_Example
{
    public partial class Error_Log : Form
    {
        public Error_Log()
        {
            InitializeComponent();
        }
        private void populateDataGridView()
        {
            try
            {
                using (OracleConnection conn = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)" +
                 "(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)" +
                 "(SERVICE_NAME=xe)));User Id=hr;Password=hr;"))
                using (OracleCommand cmd = new OracleCommand("select * from log order by error_date, num", conn))
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(reader);
                        dataGridView1.DataSource = dataTable;

                        OracleDataReader rdr = cmd.ExecuteReader();
                        // dataGridView1.Rows.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)DataGridViewAutoSizeColumnMode.Fill;
        }

        private void Error_Log_Load(object sender, EventArgs e)
        {
            populateDataGridView();
        }
    }
}
