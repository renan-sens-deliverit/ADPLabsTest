

using ADPLabs_DeliverIT_Test.API.EF;
using ADPLabs_DeliverIT_Test.API.EF.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<ICalcTaskRepository, CalcTaskRepository>();
builder.Services.AddScoped<IRequestPostCalc, RequestPostCalcRepository>();



// Add services to the container.

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();


