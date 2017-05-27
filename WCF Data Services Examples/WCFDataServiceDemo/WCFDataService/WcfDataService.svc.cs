//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace WCFDataService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class WcfDataService : DataService<AdventureWorks2012Entities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, 
            // updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("Employees", EntitySetRights.All);
            config.SetEntitySetAccessRule("*", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        // Define a query interceptor for the Employee entity set.
        [QueryInterceptor("Employees")]
        public Expression<Func<Employee, bool>> WallaWallaBingBang()
        {
            Debug.WriteLine("Firing Query Interceptor!!!");
            return e => e.Person.FirstName.StartsWith("E");
        }

        [ChangeInterceptor("Employees")]
        public void BleepBloop(Employee employee, UpdateOperations operations)
        {
            if (operations == UpdateOperations.Delete)
            {
                throw new DataServiceException(400,
                    "Employees cannot be deleted! That's cruel!");
            }
        }

        [WebGet]
        public IQueryable<Employee> RandomEmployeeQueryable()
        {
            return CurrentDataSource.Employees.OrderBy(e => Guid.NewGuid()).Take(1);
        }

        [WebGet]
        public Employee RandomEmployeeSingleton()
        {
            return CurrentDataSource.Employees.OrderBy(e => Guid.NewGuid()).First();
        }

    }
}
