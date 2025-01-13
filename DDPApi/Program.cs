using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using DDPApi.Data;
using DDPApi.Services;
using DDPApi.Interfaces;

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
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ProjectConnection")));

builder.Services.AddScoped<IStore, StoreService>();
builder.Services.AddScoped<IPerson, PersonService>();
builder.Services.AddScoped<IOrder, OrderService>();
builder.Services.AddScoped<IWork, WorkService>();
builder.Services.AddScoped<IStation, StationService>();

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
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