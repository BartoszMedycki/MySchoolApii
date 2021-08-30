using FluentValidation;
using MySchoolApiDataBase.DataModels.InDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Validators.CreateDataModelsValidators
{
   public class CreateStudentDataModelValidator : AbstractValidator<CreateStudentDataModel>
    {
        private readonly MySchoolApiDbContext dbContext;

        public CreateStudentDataModelValidator(MySchoolApiDbContext dbContext)
        {
            RuleFor(prop => prop.Name).NotEmpty();
            RuleFor(prop => prop.Surename).NotEmpty();
            RuleFor(prop => prop.Pesel).Custom((value,context)=>
            {
                var valueIsCorrect = value > 9999999999 && value <= 99999999999;
                if (!valueIsCorrect)
                {
                    context.AddFailure("Pesel", "Incorrect pesel");
                }
            
            });
            RuleFor(prop => prop.KeeperName).NotEmpty();
            RuleFor(prop => prop.KeeperSureName).NotEmpty();
            RuleFor(prop => prop.KeeperTelephoneNumber).NotEmpty();

            RuleFor(prop => prop.Email).EmailAddress().Custom((value,context)=>
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
                else context.AddFailure("Email","This Email is taken");
            
            
            });
            RuleFor(prop => prop.Password).MinimumLength(6).NotEmpty();
            RuleFor(prop => prop.ConfirmPassword).Equal(prop => prop.Password);
            this.dbContext = dbContext;
        }
    }
}
