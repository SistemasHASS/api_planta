using Planta.Api.Extensions;
using Planta.Api.Shared;
using Planta.Api.Startup;
using Planta.Application;
using Planta.Domain.Service;
using Planta.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Infraestructura de la API (logging, CORS, controllers, swagger, health, JWT)
builder.AddSerilogLogging();
builder.Services.AddCorsPolicy(builder.Configuration, builder.Environment);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddSwaggerDocs();
builder.Services.AddHealthChecks();
builder.Services.AddHostedService<DatabaseStartupCheck>();
builder.Services.AddJwtAuth(builder.Configuration, builder.Environment);

// Capas: cada una registra lo suyo (unico lugar donde Api conoce Application e Infrastructure)
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(CorsExtensions.PolicyName);

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
