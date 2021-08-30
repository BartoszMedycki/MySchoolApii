using FluentValidation;
using MySchoolApiDataBase.DataModels.InDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Validators
{
    public class CreateClassDataModelValidator : AbstractValidator<CreateClassDataModel>
    {
        public CreateClassDataModelValidator()
        {
            RuleFor(prop => prop.ClassName).NotEmpty().NotNull();
            RuleFor(prop => prop.SupervisingTeacherUniqueNumber).NotNull().NotEmpty();
        }
    }
}
