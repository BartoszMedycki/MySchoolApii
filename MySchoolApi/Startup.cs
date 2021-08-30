using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MySchoolApiDataBase;
using MySchoolApiDataBase.DataModels.InDataModels;
using MySchoolApiDataBase.Entities;
using MySchoolApiDataBase.Mappers;
using MySchoolApiDataBase.Mappers.CreateDataMappers;
using MySchoolApiDataBase.Validators;
using MySchoolApiDataBase.Validators.CreateDataModelsValidators;
using MySchoolApiDataBase.Validators.UpdateEmployeeDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySchoolApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();

            services.AddSingleton(authenticationSettings);

            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";

            }).AddJwtBearer(cfg =>
            {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
                        {
                ValidIssuer = authenticationSettings.JwtIssuer,
                ValidAudience = authenticationSettings.JwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))

            };
                
            });
            services.AddScoped<IAuthorizationHandler, StudentIsOwnerRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, EmployeeIsOwnerRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, DoesTeachTheSubjectRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, DoesTheSupervisingTeacherRequirementHandler>();
            services.AddControllers().AddFluentValidation();
            services.AddDbContext<MySchoolApiDbContext>(options => options.UseSqlServer("Server = .;Database=MySchoolApiDataBase;Trusted_Connection=true;"));
            services.AddControllersWithViews().AddNewtonsoftJson(options =>options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddScoped<CreateEmployeeMapper>();
            services.AddScoped<StudentMapper>();
            services.AddScoped<IValidator<CreateClassDataModel>, CreateClassDataModelValidator>();
            services.AddScoped<IValidator<CreateBookDataModel>, CreateBookDataModelValidator>();
            services.AddScoped<IValidator<CreateEmployeeDataModel>,CreateEmployeeDataModelValidator>();
            services.AddScoped<IValidator<CreateSchoolSubjectDataModel>, CreateSchoolSubjectDataModelValidaotr>();
            services.AddScoped<IValidator<CreateStudentDataModel>, CreateStudentDataModelValidator>();
            services.AddScoped<IValidator<UpdateEmployeeDataModel1>, UpdateEmployeeDataModelValidator>();
            services.AddScoped<CreateBookMapper>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
            services.AddScoped<BookMapper>();
            services.AddScoped<RateMapper>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ClassMapper>();
            services.AddScoped<EmployeeMapper>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<IEmployeeRepository,EmployeeRepository>();
            services.AddScoped<ClassRepository>();
            services.AddScoped<SchoolSubjectRepository>();
            services.AddScoped<StudentRepository>();
            services.AddScoped<TmpClass>();
            services.AddScoped<ExceptionHandlingMiddleWare>();
            services.AddScoped<IPasswordHasher<Student>, PasswordHasher<Student>>();
            services.AddScoped<IPasswordHasher<Employee>, PasswordHasher<Employee>>();
            services.AddHttpContextAccessor();
            services.AddScoped<IUserContextService, UserContextService>();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider,
            TmpClass tmpClass, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           var dataBase = serviceProvider.GetService(typeof(MySchoolApiDbContext)) as MySchoolApiDbContext;
            app.UseMiddleware<ExceptionHandlingMiddleWare>();
            app.UseAuthentication();
            app.UseHttpsRedirection();
           
            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            dataBase.Database.Migrate();
            tmpClass.Seed();
            
        }
    }
}
