using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApiDataBase.Entities
{
    public abstract class MainRepository<Entity> where Entity:class
    {
        protected  MySchoolApiDbContext dbContext { get; }
        protected abstract DbSet<Entity> dbSet { get; }
        public MainRepository(IServiceProvider serviceProvider)
        {
            dbContext = serviceProvider.GetService(typeof(MySchoolApiDbContext)) as MySchoolApiDbContext;
        }
        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
        public IEnumerable<Entity> getAll()
        {
            List<Entity> entities = new List<Entity>();
            var collection = dbSet;
            foreach (var item in collection)
            {
                entities.Add(item);
            }
            return entities;
        }
    }
}
