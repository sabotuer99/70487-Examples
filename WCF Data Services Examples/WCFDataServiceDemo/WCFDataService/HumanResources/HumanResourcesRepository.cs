using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace WCFDataService.HumanResources
{

    public partial class HumanResourcesRepository
    {
        AdventureWorks2012Entities _ctx;


        public HumanResourcesRepository() {
            _ctx = new AdventureWorks2012Entities();
            _ctx.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<Department> Departments
        {
            get
            {
                return _ctx.Departments.AsQueryable();
            }
        }

        public IQueryable<Employee> Employees
        {
            get
            {
                return _ctx.Employees.AsQueryable();
            }
        }

        //[WebGet]
        //public IQueryable<Department> DepartmentsWithLongNames()
        //{
        //    return _ctx.Departments.Where(d => d.Name.Length > 10).AsQueryable();
        //}
    }
}

namespace WCFDataService
{
    [DataServiceKey("DepartmentID")]
    [IgnoreProperties("EmployeeDepartmentHistories")]
    public partial class Department {}

    [DataServiceKey("BusinessEntityID")]
    [IgnoreProperties("EmployeeDepartmentHistories", "Person", "EmployeePayHistories", "JobCandidates", "PurchaseOrderHeaders", "SalesPerson")]
    public partial class Employee {

        public Department currentDepartment
        {
            get
            {
                return EmployeeDepartmentHistories.ToList().First().Department;
            }
        }
    }
}