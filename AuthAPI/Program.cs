using AuthAPI.Data;
using AuthAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization.Metadata;

namespace AuthAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Secret Key
            var secretKey = builder.Configuration["Jwt:Key"];
            var key = Encoding.ASCII.GetBytes(secretKey ?? throw new InvalidOperationException("JWT key not found in configuration"));

            // DbContext
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // JWT Auth
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            // Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AuthAPI",
                    Version = "v1",
                    Description = "API for authentication and device/entity management",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Enter 'Bearer' [space] and your token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });
            });

            // Controllers
            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.TypeInfoResolver = JsonTypeInfoResolver.Combine(
                        new DefaultJsonTypeInfoResolver()
                    );
                });


            // Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddSingleton(new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            });


            var app = builder.Build();

            // Swagger UI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthAPI v1");
                    c.RoutePrefix = "swagger";
                });
            }


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
