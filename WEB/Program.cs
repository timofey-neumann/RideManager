using System.Text;
using WEB.Middleware;
using Infrastructure;
using DAL.Persistence;
using BLL.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("RideManagerConnection");
builder.Services.AddDbContext<RideManagerContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddSwaggerGen(options =>
{
options.SwaggerDoc("v1", new OpenApiInfo
{
    Version = "v1",
    Title = "API",
    Description = ""
});
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var secretKey = builder.Configuration["JwtSettings:SecretKey"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "RideManager_AuthService",
            ValidateAudience = true,
            ValidAudience = "RideManagerApp",
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("EmployeeOnly", policy => policy.RequireRole("Employee"))
    .AddPolicy("CoordinatorOnly", policy => policy.RequireRole("Coordinator"));

builder.Services.RegisterServices();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5001")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<LoggerMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<RideManagerContext>();

    context.Database.Migrate();
}

app.MapControllers();

app.Run();
