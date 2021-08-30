using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySchoolApiDataBase.DataModels.InDataModels;

namespace MySchoolApiDataBase.Validators.UpdateEmployeeDataModel
{
   public class UpdateEmployeeDataModelValidator : AbstractValidator<UpdateEmployeeDataModel1>
    {
        enum rolessname { Teacher = 1, Director = 2, Librarian = 3 };
        public UpdateEmployeeDataModelValidator()
        {
            RuleFor(prop => prop.ContactTelephoneNumber).NotEmpty();
            RuleFor(prop => prop.Name).NotEmpty();
            RuleFor(prop => prop.SureName).NotEmpty();
           
            RuleFor(prop => prop.RoleName).IsEnumName(typeof(rolessname));
        }
    }
}
