using FluentValidation;
using MySchoolApiDataBase.DataModels.InDataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Validators.CreateDataModelsValidators
{
    public class CreateSchoolSubjectDataModelValidaotr : AbstractValidator<CreateSchoolSubjectDataModel>
    {
        public CreateSchoolSubjectDataModelValidaotr()
        {
            RuleFor(prop => prop.SubcjectName).NotEmpty();
            RuleFor(prop => prop.UniqueId).NotEmpty();
        }
    }
}
