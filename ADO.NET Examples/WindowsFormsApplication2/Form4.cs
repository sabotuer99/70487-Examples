using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Department : Form
    {

        AW_Dept ds = new AW_Dept();
        DataSet ds2 = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlCommandBuilder cmb = null;
        string connectionString;

        public Department()
        {
            InitializeComponent();
            fillDatasets();
        }

        private void fillDatasets()
        {
            ds = new AW_Dept();
            ds2 = new DataSet();

            connectionString = ConfigurationManager.
                ConnectionStrings["Default"].ConnectionString;
            using (var conn = new SqlConnection(connectionString))
            {
                initDataAdapter(conn);
                da.Fill(ds, "Department");
                da.Fill(ds2, "Department");

                //Add constraints and relations to untyped dataset
                ds2.Tables["Department"].Constraints.Add("Emp_PK",
                ds2.Tables["Department"].Columns[0], true);
            }
        }

        private void initDataAdapter(SqlConnection conn){
            da = new SqlDataAdapter(
                @"SELECT * FROM HumanResources.Department;", conn);
            da.TableMappings.Add("Table", "Department");
            cmb = new SqlCommandBuilder(da);
        }

        private void Search_Click(object sender, EventArgs e)
        {
            var id = Int32.Parse(departmentIDTextBox.Text);
            var dept = ds.Department.FindByDepartmentID((short)id);

            nameTextBox.Text = dept.Name;
            groupNameTextBox.Text = dept.GroupName;
            modifiedDateDateTimePicker.Value = dept.ModifiedDate;
        }

        private void Create_Click(object sender, EventArgs e)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        initDataAdapter(conn);

                        var dname = nameTextBox.Text;
                        var dgroup = groupNameTextBox.Text;

                        var newDept = ds.Department.NewDepartmentRow();
                        newDept.Name = dname;
                        newDept.GroupName = dgroup;
                        var mdate = DateTime.Now;
                        newDept.ModifiedDate = mdate;

                        ds.Department.Rows.Add(newDept);
                        da.Update(ds);


                        ds.Tables[0].Rows.Clear();
                        da.Fill(ds);

                        //If an exception is thrown before scope.Complete() is called, 
                        //any database changes are rolled back.
                        //throw new Exception("ERMAGERD AN EXCERPTION!!!");

                        scope.Complete();

                        //the rest does not need to the connection or transaction to work...
                        short newid = -1;
                        for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            var row = (AW_Dept.DepartmentRow)ds.Tables[0].Rows[i];
                            if (row.Name == dname &&
                               row.GroupName == dgroup &&
                               row.ModifiedDate.ToString() == mdate.ToString())
                            {
                                newid = row.DepartmentID;
                            }
                        }
                        departmentIDTextBox.Text = newid.ToString();
                        
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.Out.WriteLine(ex);
            }
        }

        private void Update_Click(object sender, EventArgs e) {

            Task<int> rowsAffected = Update_Click_Async();
            Console.Out.WriteLine(rowsAffected.Result);
        
        }

        private async Task<int> Update_Click_Async()
        {
            
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction("SampleTransaction");

                var id = (short) Int32.Parse(departmentIDTextBox.Text);
                var name = nameTextBox.Text;
                var groupName = groupNameTextBox.Text;
                var modifiedDate = DateTime.Now;
                SqlCommand cmd = new SqlCommand(
                       @"UPDATE HumanResources.Department 
                         SET Name=@name, GroupName=@gname, ModifiedDate=@mdate
                         WHERE DepartmentId = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@gname", groupName);
                cmd.Parameters.AddWithValue("@mdate", modifiedDate);
                cmd.Transaction = trans;

                try
                {
                    var x = cmd.ExecuteNonQuery();

                    trans.Commit();

                    SqlCommand select = new SqlCommand(
                        @"SELECT * FROM HumanResources.Department;", conn);
                    //select.Transaction = trans;
                    var dr = select.ExecuteReader();

                    ds.Tables[0].Load(dr);
                    return x;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        trans.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                    return 0;
                }
            }
        }

            /*
            initDataAdapter(conn);

            var id = Int32.Parse(departmentIDTextBox.Text);
            var dept = ds.Department.FindByDepartmentID((short)id);
            dept.Name = nameTextBox.Text;
            dept.GroupName = groupNameTextBox.Text;
            dept.ModifiedDate = DateTime.Now;
                
            da.Update(ds);
            */

        private void Delete_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                initDataAdapter(conn);
                conn.Open();
                cmb.GetDeleteCommand(); //generates the Delete command

                var trans = conn.BeginTransaction("TypedTransaction");

                var id = Int32.Parse(departmentIDTextBox.Text);
                var dept = ds.Department.FindByDepartmentID((short)id);
                dept.Delete();

                cmb.GetDeleteCommand().Transaction = trans;
                da.Update(ds);
                trans.Commit();

                departmentIDTextBox.Text = "";
                nameTextBox.Text = "";
                groupNameTextBox.Text = "";
                modifiedDateDateTimePicker.Value = DateTime.Now;
            }
        }
    }
}
