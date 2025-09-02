using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SQLViewer
{

    public partial class Form1 : Form
    {
        int startRow = 1;

        public Form1()
        {
            InitializeComponent();

            ReadNewRecords();

        }

        private void ReadNewRecords()
        {
            DataTable dt = new();
            //Random random = new();
            //int randomRow = random.Next(1,417700);
            string offsetRow = startRow.ToString(); //randomRow.ToString();

            try
            {
                SqlConnection conn = new(@"Data Source=.\SQLEXPRESS;Initial Catalog=AbsmartRT;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
                //Provider=.NET Framework Data Provider for SQL Server

                conn.Open();

                string sql = "select DATE_TIME," +
                                "nullif(TI1R, -999.25) as TI1R," + /*TPA In T1 (Top) Raw*/
                                "nullif(TI2R, -999.25) as TI2R," + /*TPA In T2 (Middle) Raw*/
                                "nullif(TI3R, -999.25) as TI3R," + /*TPA In T3 (Bottom) Raw*/
                                //"nullif( , -999.25) as ," + /*TI Flow Src*/
                                //"nullif( , -999.25) as ," + /*TI MW*/
                                //"nullif( , -999.25) as ," + /*TI MW (Tuned)*/
                                //"nullif( , -999.25) as ," + /*TI MW (Linear)*/
                                //"nullif( , -999.25) as ," + /*TI MW FScn*/
                                "nullif(TI12, -999.25) as TI12," + /*TPA In T1T2 Section Density*/
                                "nullif(TI23, -999.25) as TI23, " + /*TPA In T2T3 Section Density*/
                                "nullif(TI13, -999.25) as TI13 " + /*TPA In T1T3 Section Density*/
                                //"nullif( , -999.25) as , " + /*TI MW in Mud Bal*/
                                "from WITSData ORDER BY DATE_TIME DESC OFFSET " + offsetRow + " ROWS FETCH NEXT 60 ROWS ONLY";
                //"nullif(CIDF, -999.25) as CIDF," + /*Centrifuge 1 Feed Density (Tuned)*/
                //"nullif(CIFR, -999.25) as CIFR," + /*Centrifuge 1 Feed Flow Raw*/
                //"nullif(CIDR, -999.25) as CIDR," + /*Centrifuge 1 Feed Density Raw*/
                //"nullif(CNIS, -999.25) as CNIS," + /*Centrifuge 1 Feed Status*/
                //"nullif(CITF, -999.25) as CITF," + /*Centrifuge 1 Feed Mass Totalizer*/
                //"nullif(CIDL, -999.25) as CIDL," + /*Centrifuge 1 Feed Density (Linear)*/
                //"nullif(CIFS, -999.25) as CIFS," + /*Centrifuge 1 Feed Flow (Linear)*/
                //"nullif(CIFL, -999.25) as CIFL," + /*CNI Flow Linear (Corrected Density)*/
                //"nullif(CIFG, -999.25) as CIFG," + /*Centrifuge 1 Feed Flow (Tuned)*/
                //"nullif(RCCM, -999.25) as RCCM " + /*Centrifuge 1 Feed Mud Mass Rate*/
                //"from WITSData ORDER BY DATE_TIME DESC OFFSET " + offsetRow + " ROWS FETCH NEXT 60 ROWS ONLY";

                //"select DATE_TIME, R3FR, R2PM, R1ND, R2ND, R3ND, DMEA from WITSData order by DATE_TIME DESC OFFSET " + offsetRow + " ROWS FETCH NEXT 10 ROWS ONLY";
                //"select top 10 DATE_TIME, DENI, TDNL, DSNI, TI3R, TI2R, TI1R from WITSData order by DATE_TIME DESC";
                //"select top 10 * from WITSData order by DATE_TIME DESC";
                //"select * from WITSData where DATE_TIME >= '2025-04-29 23:00:00' order by DATE_TIME";
                SqlCommand cmd = new(sql, conn);
                SqlDataAdapter dp = new(cmd);

                dp.Fill(dt);

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SQL Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            //MessageBox.Show("Query finished", "Message", MessageBoxButtons.OK);


            chart1.DataSource = dt;
            chart1.Series["TI3R"].XValueMember = "DATE_TIME";
            chart1.Series["TI2R"].XValueMember = "DATE_TIME";
            chart1.Series["TI1R"].XValueMember = "DATE_TIME";
            //chart1.Series[""].XValueMember = "DATE_TIME";
            //chart1.Series[""].XValueMember = "DATE_TIME";
            chart1.Series["TI12"].XValueMember = "DATE_TIME";
            chart1.Series["TI23"].XValueMember = "DATE_TIME";
            chart1.Series["TI13"].XValueMember = "DATE_TIME";
            chart1.Series["TI3R"].YValueMembers = "TI3R";
            chart1.Series["TI2R"].YValueMembers = "TI2R";
            chart1.Series["TI1R"].YValueMembers = "TI1R";
            //chart1.Series[""].YValueMembers = "";
            //chart1.Series[""].YValueMembers = "";
            chart1.Series["TI12"].YValueMembers = "TI12";
            chart1.Series["TI23"].YValueMembers = "TI23";
            chart1.Series["TI13"].YValueMembers = "TI13";
            chart1.DataBind();


            dataGridView1.DataSource = dt;
            //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DataGridViewColumn dateTimeCol = dataGridView1.Columns[0];
            //dateTimeCol.Width = 250;
            dateTimeCol.DefaultCellStyle.Format = "G"; //Long datetime
            dataGridView1.Show();


        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            startRow += 300;
            ReadNewRecords();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //startRow -= 1;
            ReadNewRecords();
        }
    }
}
