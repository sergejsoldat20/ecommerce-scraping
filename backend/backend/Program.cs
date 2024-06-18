using backend.Data;
using backend.Extensions;
using backend.Helpers;
using backend.Security;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(
	options => options.UseNpgsql(builder.Configuration.GetConnectionString("Db"))
);

// Security configuration
builder.Services.Configure<JwtSecret>(builder.Configuration.GetSection(JwtSecret.SectionName));
builder.Services.RegisterAuthentication(builder.Configuration);
builder.Services.RegisterAuthorization();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IAuthorizationHandler, JwtAuthorizationHandler>();

// Automapper configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IAccountService, AccountService>();	
builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();