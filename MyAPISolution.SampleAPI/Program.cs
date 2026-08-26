using Microsoft.EntityFrameworkCore;
using MyAPISolution.SampleAPI.DAL;
using MyAPISolution.SampleAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//add entity framework core
builder.Services.AddDbContext<RapidDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoryDAL, CategoryMockDAL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//menambahkan custom middleware
/*app.Run(async (context)=>
{
    await context.Response.WriteAsync("Hello World!");
});*/

app.Run();