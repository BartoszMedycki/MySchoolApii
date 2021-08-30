using Microsoft.EntityFrameworkCore;
using MySchoolApiDataBase.Entities;

namespace MySchoolApiDataBase
{
   public class MySchoolApiDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Employee> Employees { get; set; }
     
        public DbSet<Role> Roles { get; set; }
        public DbSet<SchoolSubject> SchoolSubjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public MySchoolApiDbContext(DbContextOptions options) : base(options)
        {

            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().Property(prop => prop.RoleName).IsRequired();

            modelBuilder.Entity<Student>().HasOne(s => s.Class).WithMany(h => h.Students).OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Student>().Property(prop => prop.ClassID).IsRequired().HasMaxLength(2);
            modelBuilder.Entity<Student>().Property(prop => prop.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(prop => prop.Surename).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Student>().Property(prop => prop.Pesel).IsRequired();
            modelBuilder.Entity<Student>().Property(prop => prop.KeeperName).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Student>().Property(prop => prop.KeeperSureName).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Student>().Property(prop => prop.KeeperTelephoneNumber).IsRequired().HasMaxLength(11);
           

            modelBuilder.Entity<Employee>().Property(prop => prop.Name).IsRequired().HasMaxLength(20);
            modelBuilder.Entity<Employee>().Property(prop => prop.SureName).IsRequired().HasMaxLength(30);
            modelBuilder.Entity<Employee>().Property(prop => prop.RoleId).IsRequired();
            modelBuilder.Entity<Employee>().Property(prop => prop.UniqueNumber).IsRequired();
            modelBuilder.Entity<Employee>().Property(prop => prop.ContactTelephoneNumber).IsRequired()
                .HasMaxLength(9);
            modelBuilder.Entity<Employee>().Property(prop => prop.UserId).IsRequired();
        

            modelBuilder.Entity<Note>().Property(prop => prop.SubjectName).IsRequired();

            modelBuilder.Entity<Rate>().Property(prop => prop.Notee).IsRequired().HasMaxLength(1);
            modelBuilder.Entity<Rate>().Property(prop => prop.Description).IsRequired();

           // modelBuilder.Entity<SchoolSubject>().Property(prop => prop.LeaderId).IsRequired();
            modelBuilder.Entity<SchoolSubject>().Property(prop => prop.SubcjectName).IsRequired();

            modelBuilder.Entity<Book>().Property(prop => prop.IsAvailable).HasDefaultValue(true);
            modelBuilder.Entity<Book>().Property(prop => prop.title).IsRequired();
            modelBuilder.Entity<Book>().Property(prop => prop.author).IsRequired();
           



        }
      
    }
}
