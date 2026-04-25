using Microsoft.AspNetCore.HttpOverrides;
using ecomServer.Data;
using ecomServer.Middleware;
using ecomServer.Repositories;
using ecomServer.Repositories.Contracts;
using ecomServer.Services;
using ecomServer.Services.Contracts;
using ecomServer.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("ecomdb") ??
    builder.Configuration["ECOMDB_CONNECTION_STRING"];

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Database connection string is missing. Set ConnectionStrings__ecomdb or ECOMDB_CONNECTION_STRING.");
}

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var jwtKey =
    builder.Configuration["Jwt:Key"] ??
    builder.Configuration["JWT_KEY"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT key is missing. Set Jwt__Key or JWT_KEY.");
}

// Add this section to support Render's load balancer
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });


// ✅ DbContext 
builder.Services.AddDbContext<EcomDbContext>(options => options.UseSqlServer(connectionString));

// Swagger
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();          

// CORS
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientOrigins", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) 
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            ),
            RoleClaimType = "UserType" 
        };
    });

// DI
builder.Services.AddScoped<JwtUtil>();
builder.Services.AddScoped<PasswordHashUtil>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductServices, ProductServices>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartServices, CartServices>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderServices, OrderServices>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<ErrorLogService>();


var app = builder.Build();                     
app.UseForwardedHeaders();                   // Correctly handle proxy headers for Render
app.UseMiddleware<RequestTimingMiddleware>(); 
app.UseMiddleware<ExceptionMiddleware>();    
if (app.Environment.IsDevelopment())        
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();               
app.UseCors("ClientOrigins");
app.UseAuthentication();          
app.UseAuthorization();          

app.MapControllers();          
app.Run();                    
