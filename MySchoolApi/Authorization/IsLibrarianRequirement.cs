using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySchoolApiDataBase.Entities;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchoolApi
{
    public class IsLibrarianRequirement : IAuthorizationRequirement
    {
        Employee student { get; set; }
        public IsLibrarianRequirement(Employee employee)
        {
            this.student = employee;
        }
        public IsLibrarianRequirement()
        {

        }
    }
}
