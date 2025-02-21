using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using DDPApi.Data;
using DDPApi.Services;
using DDPApi.Interfaces;
using System.Text.Json.Serialization;

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

builder.Services.AddScoped<IAlert, AlertService>();
builder.Services.AddScoped<IInventoryMovement, InventoryMovementService>();
builder.Services.AddScoped<IMachine, MachineService>();
builder.Services.AddScoped<IMachineFault, MachineFaultService>();
builder.Services.AddScoped<IMaintenanceRecord, MaintenanceRecordService>();
builder.Services.AddScoped<INotification, NotificationService>();
builder.Services.AddScoped<IOrder, OrderService>();
builder.Services.AddScoped<IPerson, PersonService>();
builder.Services.AddScoped<IQualityControlRecord, QualityControlRecordService>();
builder.Services.AddScoped<IStation, StationService>();
builder.Services.AddScoped<IStore, StoreService>();
builder.Services.AddScoped<ISupplier, SupplierService>();
builder.Services.AddScoped<IWork, WorkService>();
builder.Services.AddScoped<IWorkforcePlanning, WorkforcePlanningService>();
builder.Services.AddScoped<IPositions, PositionsService>();
builder.Services.AddScoped<IStages, StagesService>();



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