using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public class RoleRepository : MainRepository<Student>
    {
        public RoleRepository(IServiceProvider service) : base(service)
        {

        }

        protected override DbSet<Student> dbSet => dbContext.Students;

        public void Seed()
        {
            var startRoles = GetStartRoles();
            if (startRoles != null)
            {
                foreach (var role in startRoles)
                {
                    dbContext.Roles.Add(role);
                }
                this.SaveChanges();
            }
        }

        public IEnumerable<Role> GetStartRoles()
        {
            List<Role> listOfStartRoles = new List<Role>()
            {
                new Role()
                {
                RoleName = "Teacher"
                },
                new Role()
                {
                RoleName = "Director"
                },
                new Role()
                {
                RoleName = "Admin"
                },
                new Role()
                {
                RoleName = "Librarian"
                },
                new Role()
                {
                RoleName = "Student"
                }
            };
            return listOfStartRoles;
        }
    }
}
