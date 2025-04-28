using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MedicalArchive.API.Application.Services;
using MedicalArchive.API.Infrastructure.Data;
using MedicalArchive.API.Infrastructure.Repositories;

namespace MedicalArchive.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Додавання DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                     builder.Configuration.GetConnectionString("DefaultConnection"),
                     b => b.MigrationsAssembly("MedicalArchive.API")
                )
            );

            // Реєстрація репозиторіїв
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
            builder.Services.AddScoped<IDoctorAccessRepository, DoctorAccessRepository>();
            builder.Services.AddScoped<IDoctorAppointmentRepository, DoctorAppointmentRepository>();
            builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            builder.Services.AddScoped<IReferralRepository, ReferralRepository>();
            builder.Services.AddScoped<IVaccinationRepository, VaccinationRepository>();
            builder.Services.AddScoped<IMedicalDocumentRepository, MedicalDocumentRepository>();

            // Реєстрація сервісів
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IDoctorAccessService, DoctorAccessService>();
            builder.Services.AddScoped<IDoctorAppointmentService, DoctorAppointmentService>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
            builder.Services.AddScoped<IReferralService, ReferralService>();
            builder.Services.AddScoped<IVaccinationService, VaccinationService>();
            builder.Services.AddScoped<IMedicalDocumentService, MedicalDocumentService>();

            // Налаштування JWT автентифікації
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"] ?? "DefaultSecretKeyForDevelopment12345"))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
                options.AddPolicy("RequireDoctorRole", policy => policy.RequireRole("Doctor"));
            });

            builder.Services.AddControllers();

            // Налаштування Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Medical Archive API", Version = "v1" });

            // Налаштування для авторизації в Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // CORS налаштування
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Налаштування HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medical Archive API v1"));
            }

            app.UseHttpsRedirection();

            // Директорія для статичних файлів (для завантаження документів)
            app.UseStaticFiles();

            // Використання CORS
            app.UseCors("AllowAll");

            // Додавання middleware автентифікації та авторизації
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            //// Автоматична міграція бази даних при запуску (опціонально)
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var context = services.GetRequiredService<ApplicationDbContext>();
            //        context.Database.Migrate();
            //    }
            //    catch (Exception ex)
            //    {
            //        var logger = services.GetRequiredService<ILogger<Program>>();
            //        logger.LogError(ex, "Помилка при міграції бази даних");
            //    }
            //}

            app.Run();
        }
    }
}