using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MySchoolApiDataBase;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.Entities;
using MySchoolApiDataBase.Mappers;
using MySchoolApiDataBase.Mappers.CreateDataMappers;
using MySchoolApiDataBase.Validators;
using MySchoolApiDataBase.Validators.CreateDataModelsValidators;
using MySchoolApiDataBase.Validators.UpdateEmployeeDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApi
{
    public enum RolesNames
    {
        Teacher,
        Director,
        Admin,
        Student,
        Librarian
    }
}