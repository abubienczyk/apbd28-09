using Microsoft.EntityFrameworkCore;
using TripsApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ScaffoldContext>(
    //jak sie cos wywali to sprobowac tego stringa "Data Source=localhost;Initial Catalog=scaffold;User ID=SA;Password=yourStrong@Password;Encrypt=False"
    options => options.UseSqlServer("Name=ConnectionStrings:Default"));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();

