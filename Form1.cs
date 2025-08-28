using System.Data;
using System.Data.OleDb;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SQLViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            OleDbConnection conn = new (@"Provider=MSOLEDBSQL.1;Data Source=LT-DELL2IN1-RS\SQLEXPRESS;Persist Security Info=False;Integrated Security=SSPI;Initial Catalog=AbsmartRT;Trust Server Certificate=True");
            //Provider = SQLOLEDB.1; Data Source = localhost\SQLEXPRESS; Integrated Security = SSPI; Initial Catalog = AbsmartRT
            InitializeComponent();

            try
            {
                conn.Open();
                OleDbCommand cmd = conn.CreateCommand();
                //cmd.CommandType = CommandType.Text;
                cmd.CommandText = "select * from WITSData";
                    //"select * from bak3_WITSData where DATE_TIME >= '2025-04-29 23:00:00' order by DATE_TIME";
                cmd.ExecuteNonQuery();
                DataTable dt = new ();
                OleDbDataAdapter dp = new (cmd);
                dp.Fill(dt);
                dataGridView1.DataSource = dt;
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Test Data", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
