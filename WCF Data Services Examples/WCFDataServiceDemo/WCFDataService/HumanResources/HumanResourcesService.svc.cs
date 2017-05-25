//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using WCFDataService.HumanResources;

namespace WCFDataService
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class HumanResourcesService : DataService<HumanResourcesRepository>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("Departments", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Employees", EntitySetRights.AllRead);
            config.SetServiceOperationAccessRule("DepartmentsWithLongNames", ServiceOperationRights.All);
            config.SetServiceOperationAccessRule("DepartmentWithLongestName", ServiceOperationRights.All);
            //config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        [WebGet]
        public IQueryable<Department> DepartmentsWithLongNames(int length)
        {
            return CurrentDataSource.Departments.Where(d => d.Name.Length >= length).AsQueryable();
        }

        [WebGet]
        public Department DepartmentWithLongestName()
        {
            //return CurrentDataSource.Departments.OrderByDescending(d => d.Name.Length).Take(1);
            return CurrentDataSource.Departments.OrderByDescending(d => d.Name.Length).First();
        }

    }
}
