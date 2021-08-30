using FluentValidation;
using MySchoolApiDataBase.DataModels.InDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Validators
{
    public class CreateEmployeeDataModelValidator : AbstractValidator<CreateEmployeeDataModel>
    {
        enum rolessname{Teacher = 1 ,Director= 2,Librarian=3,Student = 4};
        public CreateEmployeeDataModelValidator(MySchoolApiDbContext dbContext)
        {
            
            RuleFor(prop => prop.ContactTelephoneNumber).NotEmpty();
            RuleFor(prop => prop.Name).NotEmpty();
            RuleFor(prop => prop.SureName).NotEmpty();
            RuleFor(prop => prop.UniqueNumber).Custom(
                (value, context) =>
                {
                    var uniqueCodeInUse = dbContext.Employees.Any(prop => prop.UniqueNumber == value);
                    if (uniqueCodeInUse)
                    {
                        context.AddFailure("UniqueNumber", "This Unique Numbers is taken");
                    }

                });
          
            RuleFor(prop => prop.RoleName).IsEnumName(typeof(rolessname));


            RuleFor(prop => prop.Email).EmailAddress().Custom((value, context) =>
            {
                var isThatEmailTakenByStudent = dbContext.Students.Any(prop => prop.User.Email == value);
                if (!isThatEmailTakenByStudent)
                {
                    var isThatEmailTakenByEmployee = dbContext.Employees.Any(prop => prop.User.Email == value);
                    if (!isThatEmailTakenByEmployee)
                    {

                    }
                    else context.AddFailure("Email", "This email is taken");
                }
                else context.AddFailure("Email", "This Email is taken");


            });
            RuleFor(prop => prop.Password).MinimumLength(6).NotEmpty();
            RuleFor(prop => prop.ConfirmPassword).Equal(prop => prop.Password);
            

        }
    }
}
