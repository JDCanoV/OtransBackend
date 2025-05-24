using Microsoft.EntityFrameworkCore;
using OtransBackend.Repositories;
using OtransBackend.Services;
using OtransBackend.Utilities;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using OtransBackend.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

var bindJwtSettings = new JwtSettingsDto();
builder.Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
builder.Services.AddSingleton(bindJwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSigningKey,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSigningKey)),
        ValidateIssuer = bindJwtSettings.ValidateIssuer,
        ValidIssuer = bindJwtSettings.ValidIssuer,
        ValidateAudience = bindJwtSettings.ValidateAudience,
        ValidAudience = bindJwtSettings.ValidAudience,
        RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
        ValidateLifetime = bindJwtSettings.RequireExpirationTime,
        ClockSkew = TimeSpan.Zero,
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired-Time", "true");
            }
            return Task.CompletedTask;
        }
    };
});

// Base de datos
builder.Services.AddDbContext<Otrans>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cache en memoria
builder.Services.AddMemoryCache();

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
builder.Services.AddScoped<IUsuarioRepository, UserRepository>();
builder.Services.AddScoped<ITransportistaRepository,TransportistaRepository>();
builder.Services.AddScoped<IAdministradorRepository,AdministradorRepository>();
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<ITransportistaService, TransportistaService>();
builder.Services.AddScoped<IEmpresaService, EmpresaService>();
builder.Services.AddScoped<IUsuarioService, UserService>();
builder.Services.AddScoped<IAdministradorService, AdministradorService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<EmailUtility>();
builder.Services.AddScoped<JWTUtility>();
builder.Services.AddScoped<CloudinaryService, CloudinaryService>();


// GoogleDrive
builder.Services.AddScoped<GoogleDriveService, GoogleDriveService>();

// Swagger mejorado
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OtransBackend API",
        Version = "v1",
        Description = "Backend: Jhoel Blanco, Oscar Paternina<br/>Frontend: Juliana Riaño, Diego Cano"
    });
    options.EnableAnnotations();
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

app.UseAuthentication(); // <--- Muy importante que esté antes de Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();
