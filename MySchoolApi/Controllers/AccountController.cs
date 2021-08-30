using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySchoolApiDataBase;
using MySchoolApiDataBase.DataModels;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels;
using MySchoolApiDataBase.DataModels.OutDataModels.ClassDataModel;
using MySchoolApiDataBase.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MySchoolApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }


        [HttpPost("login/student")]
        public ActionResult LoginStudent([FromBody] LoginDto dto)
        {
            var token = accountRepository.GenerateJwtForStudent(dto);
            return Ok(token);
            
        }
    }
   
    
    
}