using System.Text;
using CodeCompareServer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var connectionString = $"Server={builder.Configuration["DB_HOST"]};Database={builder.Configuration["DB_DATABASE"]};User={builder.Configuration["DB_USER"]};Password={builder.Configuration["DB_PASSWORD"]};";
if (string.IsNullOrEmpty(builder.Configuration["DB_HOST"]))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string not found.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(connectionString));

var jwtSecret = builder.Configuration["ACCESS_TOKEN_SECRET"] ?? builder.Configuration["JwtSettings:Secret"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret!))
        };
    });

builder.Services.AddCors(options =>
{
    var corsOrigin = builder.Configuration["CORS_ORIGIN"] ?? "*";
    options.AddDefaultPolicy(policy =>
    {
        if (corsOrigin == "*") policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        else policy.WithOrigins(corsOrigin).AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddHttpClient("LapiClient", client =>
{
    var lapiUrl = builder.Configuration["LAPI_URL"] ?? builder.Configuration["LapiUrl"];
    client.BaseAddress = new Uri(lapiUrl!);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => new { message = "Welcome to my API. Valid routes are /userQuestions and /collections" });

app.Run();
