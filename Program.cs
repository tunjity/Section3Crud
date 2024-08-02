//using FluentValidation;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.EntityFrameworkCore;
//using Section3Crud.Model;

//var builder = WebApplication.CreateBuilder(args);

//string? conn = builder.Configuration.GetConnectionString("DefaultConnection");
//var app = builder.Build();
//builder.Services.AddEndpointsApiExplorer();

//app.UseSwagger();
////app.UseSwaggerUI();

//builder.Services.AddControllers();

//builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseSqlServer(conn));
//builder.Services.AddScoped<IValidator<CustomerFormModel>, CustomerFormModelValidator>();
//builder.Services.AddScoped<IValidator<ProductFormModel>, ProductFormModelValidator>();

//builder.Services.AddScoped<IMongoRepository, MongoRepository>();
//builder.Services.AddScoped<IMsSqlRepository, MsSqlRepository>();

//app.MapGet("/", () => "Hello World!");

//app.Run();

using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Section3Crud.Model;
using System.Reflection;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

IConfiguration config = builder.Configuration;
string? conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApiDbContext>(opt => opt.UseSqlServer(conn));
builder.Services.AddScoped<IValidator<CustomerFormModel>, CustomerFormModelValidator>();
builder.Services.AddScoped<IValidator<ProductFormModel>, ProductFormModelValidator>();

builder.Services.AddScoped<IMongoRepository, MongoRepository>();
builder.Services.AddScoped<IMsSqlRepository, MsSqlRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // Allow any origin
            .AllowCredentials() // Allow credentials
            .WithExposedHeaders("Content-Disposition");
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    // Optionally include XML comments for better documentation (uncomment and adjust the path as needed)
    // var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = string.Empty; 
});

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthorization();

app.MapControllers();

app.Run();

