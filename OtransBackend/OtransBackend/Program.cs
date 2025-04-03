using Microsoft.EntityFrameworkCore;
using OtransBackend.Repositories;
using OtransBackend.Services;
using OtransBackend.Utilities;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Base de datos
builder.Services.AddDbContext<Otrans>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressInferBindingSourcesForParameters = true;
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


// Inyección de dependencias
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();

// GoogleDrive
builder.Services.AddScoped<GoogleDriveService>(provider =>
{
    var credentialPath = Path.Combine(Directory.GetCurrentDirectory(), "Utilities", "client_secret_741082527421-1e8jasp95vkv6b20sqo8i2pfh7kio26v.apps.googleusercontent.com");
    return new GoogleDriveService(credentialPath);
});
builder.Services.AddScoped<GoogleDriveRepository>();

// Swagger mejorado
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OtransBackend API",
        Version = "v1"
    });

    // Configuración para manejar archivos
    options.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    // Filtro personalizado para documentación de archivos
    options.OperationFilter<SwaggerFileUploadFilter>();
});

// Controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

// Middleware
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "OtransBackend V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
