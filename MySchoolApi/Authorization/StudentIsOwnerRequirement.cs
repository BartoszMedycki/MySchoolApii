using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchoolApi
{
    public class StudentIsOwnerRequirement : IAuthorizationRequirement
    {
        public int id;
        public StudentIsOwnerRequirement(int id)
        {
            this.id = id;
        }
        public StudentIsOwnerRequirement()
        {

        }
    }
}
