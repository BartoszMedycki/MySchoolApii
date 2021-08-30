using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchoolApi
{
    public class BadEmailOrPasswordException : Exception
    {
        public BadEmailOrPasswordException(string msg) : base(msg)
        {

        }
    }
}
