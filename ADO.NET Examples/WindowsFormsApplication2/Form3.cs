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
        string connectionString;

        public Form3()
        {
            InitializeComponent();
            
            connectionString = ConfigurationManager.
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

        //Create
        private void buttonCreate_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var firstName = textboxFirstName.Text;
                var lastName = textBoxLastName.Text;
                var maritalStatus = textBoxMaritalStatus.Text;
                var gender = textBoxGender.Text;


                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  INSERT INTO Person.BusinessEntity (rowguid, ModifiedDate)
                        VALUES (NEWID(), GETDATE());

                        DECLARE @id int;
                        SET @id = SCOPE_IDENTITY();

                        INSERT INTO Person.Person (BusinessEntityId, FirstName, LastName, 
                            PersonType, NameStyle, EmailPromotion, rowguid, ModifiedDate)
                        VALUES (@id, @fname, @lname, 'EM', 0, 0, NEWID(), GETDATE());

                        INSERT INTO HumanResources.Employee (BusinessEntityId, MaritalStatus, Gender,
                            NationalIdNumber, 
                            LoginID, JobTitle, BirthDate, HireDate, SalariedFlag,
                            VacationHours, SickLeaveHours, CurrentFlag, rowguid, ModifiedDate)
                        VALUES (@id, @mstat, @gender, 
                            SUBSTRING(CONVERT(NVARCHAR(50), NEWID()),1, 15), 
                            @fname + @lname + '@example.com', '', '1982-01-01', GETDATE(), 0, 
                            0, 0, 1, NEWID(), GETDATE());

                        SELECT @id";
                cmd.Parameters.AddWithValue("@fname", firstName);
                cmd.Parameters.AddWithValue("@lname", lastName);
                cmd.Parameters.AddWithValue("@mstat", maritalStatus);
                cmd.Parameters.AddWithValue("@gender", gender);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                var id = cmd.ExecuteScalar();
                textboxId.Text = id.ToString();
                conn.Close();
            }
        }

        //Update
        private void buttonUpdate_Click(object sender, EventArgs e)
        {

        }

        //Delete
        private void buttonDelete_Click(object sender, EventArgs e)
        {

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
