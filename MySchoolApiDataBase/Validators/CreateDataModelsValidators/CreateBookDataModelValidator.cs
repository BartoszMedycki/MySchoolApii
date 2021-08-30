using FluentValidation;
using MySchoolApiDataBase.DataModels.InDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Validators
{
    public class CreateBookDataModelValidator : AbstractValidator<CreateBookDataModel>
    {
        public CreateBookDataModelValidator()
        {
            RuleFor(prop => prop.title).NotEmpty();
            RuleFor(prop => prop.author).NotEmpty();
        }
    }
}
