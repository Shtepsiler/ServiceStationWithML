using PARTS.DAL;
using PARTS.BLL;
using Microsoft.EntityFrameworkCore;
using PARTS.DAL.Data;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PARTS.DAL.Seeders;
using PARTS.DAL.Entities.Vehicle;
using PARTS.DAL.Entities.Item;
namespace PARTS.API;
public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);



        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
 
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Parts Api" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme.",
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        });

        builder.Services.AddDbContext<PartsDBContext>(options =>
        {
            string connectionString;

            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {

                var dbhost = Environment.GetEnvironmentVariable("DB_HOST");
                var dbname = Environment.GetEnvironmentVariable("DB_NAME");
                var dbuser = Environment.GetEnvironmentVariable("DB_USER");
                var dbpass = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");


                connectionString = $"Data Source={dbhost};User ID={dbuser};Password={dbpass};Initial Catalog={dbname};Encrypt=True;Trust Server Certificate=True;";
            }
            else
                connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            options.UseSqlServer(connectionString);
            
        });

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            string redisConfiguration = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                ? Environment.GetEnvironmentVariable("REDIS")
                : builder.Configuration.GetValue<string>("Redis");

            if (string.IsNullOrEmpty(redisConfiguration))
            {
                throw new ArgumentException("No Redis configuration specified.");
            }

            options.Configuration = redisConfiguration;
            options.InstanceName = "ServiceStationParts";
        });



        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddCookie("Identity.Application", options =>
            {
                options.Cookie.Name = "Bearer";
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"])),
                    ClockSkew = TimeSpan.FromDays(1),
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["Bearer"];
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();
        builder.Services.AddPartsDal();
        builder.Services.AddPartsBll();



        var app = builder.Build();
  
        using (var scope = app.Services.CreateAsyncScope())
        {
            var dbcontext = scope.ServiceProvider.GetRequiredService<PartsDBContext>();

            ModelSplitter modelSplitter = new(dbcontext);
            if (!modelSplitter.isDataPresent())
                modelSplitter.Seed();
            modelSplitter.seedVehicles();

            ShopSeeder shopSeeder = new ShopSeeder(dbcontext);
            shopSeeder.Seed();
        }
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}