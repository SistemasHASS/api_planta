using api_planta.Application.UseCaseImpl;
using api_planta.Domain.Repository;
using api_planta.Domain.UseCase;
using api_planta.Infraestructure.Data;
using api_planta.Infraestructure.RepositoryImpl;
using api_planta.Infraestructure.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddDbContextPool<ApplicationDbContext>(opciones => opciones.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPlantaRepository, PlantaRepositoryImpl>();
builder.Services.AddScoped<IPlantaUseCase, PlantaUseCaseImpl>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Servicio API v1");
    c.DocumentTitle = "Servicio Transporte API Docs";
});

app.UseAuthorization();

app.MapControllers();

app.Run();
