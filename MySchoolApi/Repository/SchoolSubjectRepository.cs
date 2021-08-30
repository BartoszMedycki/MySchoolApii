using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class SchoolSubjectRepository : MainRepository<SchoolSubject>
    {
        public SchoolSubjectRepository(IServiceProvider service) : base(service)
        {

        }

        protected override DbSet<SchoolSubject> dbSet => dbContext.SchoolSubjects;
        public void Seed()
        {
            
            if (!dbContext.SchoolSubjects.Any())
            {
                var startSchoolSubjects = getStartSchoolSubjects();
                if (startSchoolSubjects != null)
                {
                    foreach (var schoolSubject in startSchoolSubjects)
                    {
                        dbContext.SchoolSubjects.Add(schoolSubject);
                        this.SaveChanges();
                    }
                }

            }
        }

        public IEnumerable<SchoolSubject> getStartSchoolSubjects()
        {
            List<SchoolSubject> listOfStartSchollSubjects = new List<SchoolSubject>()
        {
            new SchoolSubject()
            {
            SubcjectName = "English",
            Leader = dbContext.Employees.Where(employee=>employee.UniqueNumber == 9997).FirstOrDefault()
            },
            new SchoolSubject()
            {
            SubcjectName = "Math",
            Leader = dbContext.Employees.Where(employee=>employee.UniqueNumber == 9998).FirstOrDefault()
            }
        };
            return listOfStartSchollSubjects;
        }
    }
}
