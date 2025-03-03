using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using DDPApi.Data;
using DDPApi.Services;
using DDPApi.Interfaces;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container
builder.Services.AddControllers();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

// Add Authorization
builder.Services.AddAuthorization();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProjectConnection")));

builder.Services.AddScoped<IMachine, MachineService>();
builder.Services.AddScoped<IMachineFault, MachineFaultService>();
builder.Services.AddScoped<IMaintenanceRecord, MaintenanceRecordService>();
builder.Services.AddScoped<IOrder, OrderService>();
builder.Services.AddScoped<IPerson, PersonService>();
builder.Services.AddScoped<IQualityControlRecord, QualityControlRecordService>();
builder.Services.AddScoped<IStation, StationService>();
builder.Services.AddScoped<IStore, StoreService>();
builder.Services.AddScoped<IWork, WorkService>();
builder.Services.AddScoped<IPositions, PositionsService>();
builder.Services.AddScoped<IStages, StagesService>();
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DDP API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
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
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<DataUpdateHub>("/dataUpdateHub");

app.Run();

public class DataUpdateHub : Hub
{
    public async Task SendDataUpdate(string data)
    {
        await Clients.All.SendAsync("ReceiveDataUpdate", data);
    }
}