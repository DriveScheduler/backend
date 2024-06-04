using API.Authentication;

using Application;

using Domain.Models;
using Domain.Models.Users;
using Domain.Models.Vehicles;
using Domain.Repositories;

using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        //var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        //optionsBuilder.UseSqlite("Data Source=C:\\Users\\Romain\\Documents\\UnitTests\\DriveScheduler\\backend\\src\\Infrastructure\\database.db");
        //DatabaseContext db = new DatabaseContext(optionsBuilder.Options);
        //IUserRepository userRepository = new UserRepository(db);
        //ILessonRepository lessonRepository = new LessonRepository(db);
        //IVehicleRepository vehicleRepository = new VehicleRepository(db);

        //Teacher teacher = new Teacher("name", "test", "test@gmail.com", "psw", Domain.Enums.LicenceType.Car);
        //Teacher teacher2 = new Teacher("name2", "test2", "test2@gmail.com", "psw", Domain.Enums.LicenceType.Car);
        //Student student = new Student("student", "student", "student@gmail.com", "psw", Domain.Enums.LicenceType.Car);
        //Student student1 = new Student("studentWait", "studentWait", "studentWait@gmail.com", "psw", Domain.Enums.LicenceType.Car);
        //Student student2 = new Student("studentWait2", "studentWait2", "studentWait2@gmail.com", "psw", Domain.Enums.LicenceType.Car);
        //Car vehicle = new Car("AA123BB", "voiture");
        //Car vehicle2 = new Car("CC123DD", "voiture2");
        //Lesson lesson = new Lesson("lesson", DateTime.Now, 60, teacher, Domain.Enums.LicenceType.Car, vehicle);

        //userRepository.Insert([teacher, teacher2, student, student1, student2]);
        //lesson.AddStudent(student);
        //lesson.AddStudentToWaitingList(student1);
        //lesson.AddStudentToWaitingList(student2);
        //vehicleRepository.Insert([vehicle, vehicle2]);
        //lessonRepository.Insert(lesson);


        //Lesson lessonV2 = lessonRepository.GetById(lesson.Id);
        //lessonV2.RemoveStudentFromWaitingList(student1);
        //lessonV2.RemoveStudentFromWaitingList(student2);
        //lessonV2.RemoveStudent();
        //lessonV2.Update("lessonV2", DateTime.Now.AddDays(1), 120, teacher2, vehicle2);
        //lessonRepository.Update(lessonV2);


        //Lesson lessonV3 = lessonRepository.GetById(lesson.Id);
        //lessonRepository.Delete(lessonV3);

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.SetupDatabase($"Data Source={builder.Configuration.GetConnectionString("Database")}");

        builder.Services.ApplicationMediator();
        builder.Services.InfrastructureDependencyInjection();

        builder.Services.AddJwtTokenService(builder.Configuration);
        builder.Services.AddSwaggerJwtTokenService();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();            
            app.UseSwaggerUI();
        }

        app.UseCors(builder =>
           builder
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader());


        app.UseAuthentication();        
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}