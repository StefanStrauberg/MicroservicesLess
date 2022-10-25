using Discount.API.Extensions;
using Discount.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MigrationDatabase<Program>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
