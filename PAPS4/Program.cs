using Microsoft.Extensions.DependencyInjection.Extensions;
using PAPS4.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWellService, WellService>();

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

public class WellGenerator
{
    private readonly Random _random = new Random();

    public List<WellDto> GenerateWells(int numberOfWells)
    {
        var wells = new List<WellDto>();

        for (var i = 0; i < numberOfWells; i++)
        {
            var well = new WellDto
            {
                Id = i + 1,
                Name = $"Well_{i + 1}",
                Depth = _random.Next(100, 1000)  // Глубина скважины в пределах от 100 до 1000 метров (пример)
            };

            wells.Add(well);
        }

        return wells;
    }
}
