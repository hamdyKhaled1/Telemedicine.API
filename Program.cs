using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
using Telemedicine.API.Hubs;
using Telemedicine.API.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TelemedicineDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
QuestPDF.Settings.License = LicenseType.Community;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<WaitingRoomHub>("/waitingRoomHub");

app.Run();
