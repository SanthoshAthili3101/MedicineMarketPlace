using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MedicineMarketPlace.Shared.Context;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.BuildingBlocks;
using System.Reflection;
using FluentValidation.AspNetCore;
using MedicineMarketPlace.BuildingBlocks.Extensions;
using MedicineMarketPlace.Admin.Infrastructure.DatabaseMigrations;
using MedicineMarketPlace.Admin.Infrastructure.SeedData;
using Microsoft.AspNetCore.Mvc;
using MedicineMarketPlace.Admin.Application;
using MedicineMarketPlace.BuildingBlocks.MiddleWare;
using MedicineMarketPlace.Admin.Application.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddDbContext<MedicineMarketDbContext>(options =>
    options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 28))));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<MedicineMarketDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddJwtAuthentication(configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(ValidateModelStateAttribute));
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
                          policy =>
                          {
                              policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                          });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Medicine Market Place Admin API", Version = "v1" });

    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Auth Bearer Scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddFluentValidationAutoValidation();
//builder.Services.AddValidatorsFromAssemblyContaining(typeof(ForgotPasswordValidator));
builder.Services.AddAutoMapper(new Assembly[]
{
    Assembly.GetAssembly(typeof(Program)),
    Assembly.GetAssembly(typeof(ModelMapper))
});

builder.Services.MMPAdminServices();
builder.Services.AddDbHealthCheck(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Medicine Market Place Admin API v1");
    });
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks($"/health");

//app.UseMiddleware<ExceptionMiddleware>();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await services.MigrateAsync();
    await services.SeedUsersAndRoles();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

app.Run();
