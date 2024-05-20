using Application;

using Infrastructure;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.SetupDatabase($"Data Source={builder.Configuration.GetConnectionString("Database")}");

        builder.Services.ApplicationMediator();
        builder.Services.InfrastructureDependencyInjection();


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

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}