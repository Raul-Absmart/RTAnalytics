using System.Data;
using System.Data.OleDb;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SQLViewer
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ReadNewRecords(); 
            
        }

        private void ReadNewRecords()
        {
            DataTable dt = new();
            //int rowCount;
            
            try
            {
                OleDbConnection conn = new(@"Provider=MSOLEDBSQL.1;Data Source=localhost\SQLEXPRESS;Persist Security Info=False;Integrated Security=SSPI;Initial Catalog=AbsmartRT;Trust Server Certificate=True");
                //Provider = SQLOLEDB.1; Data Source = localhost\SQLEXPRESS; Integrated Security = SSPI; Initial Catalog = AbsmartRT
                //Data Source=.\SQLEXPRESS;Initial Catalog=AbsmartRT;Integrated Security=True;Encrypt=False;Trust Server Certificate=True
                //Provider=.NET Framework Data Provider for SQL Server

                conn.Open();

                OleDbCommand cmd = conn.CreateCommand();
                //cmd.CommandType = CommandType.Text;
                cmd.CommandText = //"select top 10 DATE_TIME, DENI, TDNL, DSNI, TI3R, TI2R, TI1R from WITSData order by DATE_TIME DESC";
                "select top 10 * from WITSData order by DATE_TIME DESC";
                //"select * from bak3_WITSData where DATE_TIME >= '2025-04-29 23:00:00' order by DATE_TIME";
                //rowCount =
                cmd.ExecuteNonQuery();
                OleDbDataAdapter dp = new(cmd);
                dp.Fill(dt);

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Data", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DataGridViewColumn dateTimeCol = dataGridView1.Columns[0];
            //dateTimeCol.Width = 250;
            dateTimeCol.DefaultCellStyle.Format = "G"; //Long datetime
            //lblRowCount = Convert(string, rowCount);

        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            ReadNewRecords();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //private void lblRowCount_Click(object sender, EventArgs e)
        //{

        //}
    }
}
