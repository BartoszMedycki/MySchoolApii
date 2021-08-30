using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class TmpClass : MainRepository<Student>
    {
        private readonly IServiceProvider service;
        private readonly RoleRepository roleRepository;
        private readonly ClassRepository classRepository;
        private readonly IEmployeeRepository employeeRepository;
        private readonly SchoolSubjectRepository schoolSubjectRepository;
        private readonly StudentRepository studentRepository;
        private readonly IServiceProvider serviceProvider;
        private readonly MySchoolApiDbContext database;

        public TmpClass(IServiceProvider service,RoleRepository roleRepository,
            ClassRepository classRepository, IEmployeeRepository employeeRepository,
            SchoolSubjectRepository schoolSubjectRepository, StudentRepository studentRepository,
            IServiceProvider serviceProvider) : base(service)
        {
            this.service = service;
            this.roleRepository = roleRepository;
            this.classRepository = classRepository;
            this.employeeRepository = employeeRepository;
            this.schoolSubjectRepository = schoolSubjectRepository;
            this.studentRepository = studentRepository;
            this.serviceProvider = serviceProvider;
            database = serviceProvider.GetService(typeof(MySchoolApiDbContext)) as MySchoolApiDbContext;
        }
        
        protected override DbSet<Student> dbSet => dbContext.Students;
        public void Seed()
        {
            roleRepository.Seed();
            employeeRepository.Seed();
            schoolSubjectRepository.Seed();
           
            
                classRepository.Seed();
            dbContext.SaveChanges();

                studentRepository.Seed();
            
            
            dbContext.SaveChanges();
        }
    }
}
