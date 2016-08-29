using System;
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
    public partial class Form3 : Form
    {
        AdventureWorksDS ds = new AdventureWorksDS();
        DataSet ds2 = new DataSet();

        public Form3()
        {
            InitializeComponent();
            
            var connectionString = ConfigurationManager.
                ConnectionStrings["Default"].ConnectionString;
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter(
                    @"SELECT * FROM HumanResources.Employee; 
                      SELECT * FROM Person.Person", conn);
                da.TableMappings.Add("Table", "Employee");
                da.TableMappings.Add("Table1", "Person");
                da.Fill(ds);
                da.Fill(ds2);

                //Add constraints and relations to untyped dataset
                ds2.Tables["Employee"].Constraints.Add("Emp_PK", 
                    ds2.Tables["Employee"].Columns[0], true);
                ds2.Tables["Person"].Constraints.Add("Person_PK", 
                    ds2.Tables["Person"].Columns[0], true);
                string[] columns = {"BusinessEntityId"};
                var relation = new DataRelation("FK_Person_Employee", 
                    ds2.Tables["Person"].Columns[0],
                    ds2.Tables["Employee"].Columns[0]);
                ds2.Relations.Add(relation);
            }
        }

        //Typed DataSet
        private void button1_Click(object sender, EventArgs e)
        {
            resetFields();

            var id = Int32.Parse(textboxId.Text);
            var employee = ds.Employee.FindByBusinessEntityID(id);
            var person = employee.PersonRow;

            textboxFirstName.Text = person.FirstName;
            textBoxLastName.Text = person.LastName;
            textBoxMaritalStatus.Text = employee.MaritalStatus;
            textBoxGender.Text = employee.Gender;

        }

        private void buttonUTSearch_Click(object sender, EventArgs e)
        {
            resetFields();

            var id = Int32.Parse(textboxId.Text);
            var employee = ds2.Tables["Employee"].Rows.Find(id);
            var person = employee.GetParentRow(ds2.Relations[0]); 

            textboxFirstName.Text = person["FirstName"].ToString();
            textBoxLastName.Text = person["LastName"].ToString();
            textBoxMaritalStatus.Text = employee["MaritalStatus"].ToString();
            textBoxGender.Text = employee["Gender"].ToString();
        }

        private void resetFields(){
            textboxFirstName.Text = "";
            textBoxLastName.Text = "";
            textBoxMaritalStatus.Text = "";
            textBoxGender.Text = "";
        }
    }
}
/*
namespace Fake
{

    using WindowsFormsApplication2;
    private class FormX : Form { 
    
        
        

        public FormX()
        {

            AdventureWorksDS ds = new AdventureWorksDS();
            var connectionString = ConfigurationManager.
                ConnectionStrings["Default"].ConnectionString;
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter(
                    @"SELECT * FROM HumanResources.Employee; 
                      SELECT * FROM Person.Person", conn);
                da.TableMappings.Add("Table", "Employee");
                da.TableMappings.Add("Table1", "Person");
                da.Fill(ds);
            }
        }

        public FormX()
        {

            DataSet ds2 = new DataSet();
            var connectionString = ConfigurationManager.
                ConnectionStrings["Default"].ConnectionString;
            using (var conn = new SqlConnection(connectionString))
            {
                var da = new SqlDataAdapter(
                    @"SELECT * FROM HumanResources.Employee; 
                      SELECT * FROM Person.Person", conn);
                da.TableMappings.Add("Table", "Employee");
                da.TableMappings.Add("Table1", "Person");
                da.Fill(ds2);

                //Add constraints and relations to untyped dataset
                ds2.Tables["Employee"].Constraints.Add("Emp_PK",
                    ds2.Tables["Employee"].Columns[0], true);
                ds2.Tables["Person"].Constraints.Add("Person_PK",
                    ds2.Tables["Person"].Columns[0], true);
                string[] columns = { "BusinessEntityId" };
                var relation = new DataRelation("FK_Person_Employee",
                    ds2.Tables["Person"].Columns[0],
                    ds2.Tables["Employee"].Columns[0]);
                ds2.Relations.Add(relation);
            }
        }
    
    }
}*/
