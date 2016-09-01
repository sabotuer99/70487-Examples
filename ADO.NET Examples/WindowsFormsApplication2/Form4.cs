﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Department : Form
    {

        AW_Dept ds = new AW_Dept();
        DataSet ds2 = new DataSet();
        SqlDataAdapter da = new SqlDataAdapter();
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
            var cmb = new SqlCommandBuilder(da);
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

        private void Update_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                initDataAdapter(conn);

                var id = Int32.Parse(departmentIDTextBox.Text);
                var dept = ds.Department.FindByDepartmentID((short)id);
                dept.Name = nameTextBox.Text;
                dept.GroupName = groupNameTextBox.Text;
                dept.ModifiedDate = DateTime.Now;
                
                da.Update(ds);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                initDataAdapter(conn);

                var id = Int32.Parse(departmentIDTextBox.Text);
                var dept = ds.Department.FindByDepartmentID((short)id);
                dept.Delete();

                da.Update(ds);

                departmentIDTextBox.Text = "";
                nameTextBox.Text = "";
                groupNameTextBox.Text = "";
                modifiedDateDateTimePicker.Value = DateTime.Now;
            }
        }
    }
}