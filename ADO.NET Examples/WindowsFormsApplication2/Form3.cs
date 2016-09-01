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
        SqlDataAdapter da = new SqlDataAdapter();
        string connectionString;

        public Form3()
        {
            InitializeComponent();

            fillDatasets();
        }

        private void fillDatasets() {
            ds = new AdventureWorksDS();
            ds2 = new DataSet();

            connectionString = ConfigurationManager.
                ConnectionStrings["Default"].ConnectionString;
            using (var conn = new SqlConnection(connectionString))
            {
                initDataAdapter(conn);
                da.Fill(ds);
                da.Fill(ds2);

                //Add constraints and relations to untyped dataset
                ds2.Tables["Employee"].Constraints.Add("Emp_PK",
                    ds2.Tables["Employee"].Columns[0], true);
                ds2.Tables["Person"].Constraints.Add("Person_PK",
                    ds2.Tables["Person"].Columns[0], true);
                ds2.Tables["BusinessEntity"].Constraints.Add("BusinessEntity_PK",
                    ds2.Tables["BusinessEntity"].Columns[0], true);
                string[] columns = { "BusinessEntityId" };
                var relation = new DataRelation("FK_Person_Employee",
                    ds2.Tables["Person"].Columns[0],
                    ds2.Tables["Employee"].Columns[0]);
                var relation2 = new DataRelation("FK_BusinessEntity_Person",
                    ds2.Tables["BusinessEntity"].Columns[0],
                    ds2.Tables["Person"].Columns[0]);
                ds2.Relations.Add(relation);
                ds2.Relations.Add(relation2);
            }
        
        }

        private void initDataAdapter(SqlConnection conn){
            da = new SqlDataAdapter(
                @"SELECT * FROM HumanResources.Employee; 
                  SELECT * FROM Person.Person;
                  SELECT * FROM Person.BusinessEntity", conn);
            da.TableMappings.Add("Table", "Employee");
            da.TableMappings.Add("Table1", "Person");
            da.TableMappings.Add("Table2", "BusinessEntity");
            var cmb = new SqlCommandBuilder(da);
        }

        private void initBusinessEntityDataAdapter(SqlConnection conn){
            da = new SqlDataAdapter(
                @"SELECT * FROM Person.BusinessEntity", conn);
            da.TableMappings.Add("Table", "BusinessEntity");
            var cmb = new SqlCommandBuilder(da);
        }

        private void initPersonDataAdapter(SqlConnection conn)
        {
            da = new SqlDataAdapter(
                @"SELECT * FROM Person.Person", conn);
            da.TableMappings.Add("Table", "Person");
            var cmb = new SqlCommandBuilder(da);
        }

        private void initEmployeeDataAdapter(SqlConnection conn)
        {
            da = new SqlDataAdapter(
                @"SELECT * FROM HumanResources.Employee", conn);
            da.TableMappings.Add("Table", "Employee");
            var cmb = new SqlCommandBuilder(da);
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
                //initDataAdapter(conn);
                initBusinessEntityDataAdapter(conn);

                var firstName = textboxFirstName.Text;
                var lastName = textBoxLastName.Text;
                var maritalStatus = textBoxMaritalStatus.Text;
                var gender = textBoxGender.Text;

                var be = ds.BusinessEntity.NewBusinessEntityRow();
                be.rowguid = Guid.NewGuid();
                be.ModifiedDate = DateTime.Now;
                //be.BusinessEntityID = ds.BusinessEntity.Rows.Count + 1;
                ds.BusinessEntity.Rows.Add(be);
                textboxId.Text = be.BusinessEntityID.ToString();
                Console.Out.WriteLine(be.BusinessEntityID.ToString());
                //ds.AcceptChanges();
                da.Update(ds.BusinessEntity);

                initPersonDataAdapter(conn);
                var per = ds.Person.NewPersonRow();
                per.FirstName = firstName;
                per.LastName = lastName;
                per.PersonType = "EM";
                per.NameStyle = false;
                per.EmailPromotion = 0;
                per.rowguid = Guid.NewGuid();
                per.ModifiedDate = DateTime.Now;
                //per.BusinessEntityID = be.BusinessEntityID;
                per.BusinessEntityRow = be;
                ds.Person.Rows.Add(per);
                da.Update(ds.Person);

                initEmployeeDataAdapter(conn);
                var emp = ds.Employee.NewEmployeeRow();
                emp.Gender = gender;
                emp.MaritalStatus = maritalStatus;
                emp.NationalIDNumber = Guid.NewGuid().ToString().Substring(0, 15);
                emp.LoginID = firstName + lastName + "@example.com";
                emp.BirthDate = new DateTime(1982, 1, 1);
                emp.JobTitle = "";
                emp.HireDate = DateTime.Now;
                emp.SalariedFlag = false;
                emp.VacationHours = 0;
                emp.SickLeaveHours = 0;
                emp.CurrentFlag = true;
                emp.rowguid = Guid.NewGuid();
                emp.ModifiedDate = new DateTime(2015, 1, 1);
                //emp.BusinessEntityID = be.BusinessEntityID;
                emp.PersonRow = per;
                emp.BusinessEntityID = per.BusinessEntityRow.BusinessEntityID;
                ds.Employee.Rows.Add(emp);
                da.Update(ds.Employee);
            }
            









            /*
            DataRow dr = ds.Tables["Person"].NewRow();
            dr["BusinessEntityId"] = 1;
            dr["FirstName"] = firstName;
            dr["LastName"] = lastName;

            ds.Tables["Person"].Rows.Add(dr);

            da.Update(ds);




            /*
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

            fillDatasets();*/
        }

        //Update
        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var id = Int32.Parse(textboxId.Text);
                var firstName = textboxFirstName.Text;
                var lastName = textBoxLastName.Text;
                var maritalStatus = textBoxMaritalStatus.Text;
                var gender = textBoxGender.Text;


                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  UPDATE Person.Person 
                        SET FirstName = @fname, 
                            LastName = @lname
                        WHERE BusinessEntityId = @id;

                        UPDATE HumanResources.Employee
                        SET MaritalStatus = @mstat, 
                            Gender = @gender
                        WHERE BusinessEntityId = @id;";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@fname", firstName);
                cmd.Parameters.AddWithValue("@lname", lastName);
                cmd.Parameters.AddWithValue("@mstat", maritalStatus);
                cmd.Parameters.AddWithValue("@gender", gender);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            fillDatasets();
        }

        //Delete
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                var id = Int32.Parse(textboxId.Text);
                var firstName = textboxFirstName.Text;
                var lastName = textBoxLastName.Text;
                var maritalStatus = textBoxMaritalStatus.Text;
                var gender = textBoxGender.Text;


                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText =
                    @"  DELETE FROM HumanResources.Employee 
                        WHERE BusinessEntityId = @id;

                        DELETE FROM Person.Person
                        WHERE BusinessEntityId = @id;

                        DELETE FROM Person.BusinessEntity
                        WHERE BusinessEntityId = @id;";

                cmd.Parameters.AddWithValue("@id", id);
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            fillDatasets();
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
