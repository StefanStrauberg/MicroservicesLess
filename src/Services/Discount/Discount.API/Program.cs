using Discount.API.Extensions;
using Discount.API.Repositories;

const string _policyName = "CorsPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _policyName, builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithExposedHeaders("X-Pagination");
    });
});

var app = builder.Build();
app.MigrationDatabase<Program>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();
app.UseCors(_policyName);
app.MapControllers();

app.Run();
