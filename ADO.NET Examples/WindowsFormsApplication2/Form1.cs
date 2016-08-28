using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            var conn = new SqlConnection(connectionString);

            //DisconnectedModelLoad(conn);
            ConnectedModelLoad(conn);
        }
 
        private void DisconnectedModelLoad(SqlConnection conn){

            //Disconnected Model
            var da = new SqlDataAdapter("SELECT * FROM HumanResources.Employee", conn);
            var cmb = new SqlCommandBuilder(da);
            DataSet employees = new DataSet();
            da.Fill(employees, "HumanResources.Employee");
            dataGridViewEmployee.DataSource = employees.Tables[0];
        }

        private void ConnectedModelLoad(SqlConnection conn){
            //Connected Model
            var cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM HumanResources.Employee";
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            var dr = cmd.ExecuteReader();
            var employees = new DataTable();
            employees.Load(dr);
            dataGridViewEmployee.DataSource = employees;
            conn.Close();
        }
    }
}
