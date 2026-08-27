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

//add automapper
builder.Services.AddAutoMapper(cfg => cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzg1NDU2MDAwIiwiaWF0IjoiMTc1Mzk1MjI4OSIsImFjY291bnRfaWQiOiIwMTk4NWZiMTRkZTM3NTI5OWY5NTdjOTNkNmZiNmFlZiIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazFmdjgwbmdtemFkOHlzdGhqYmUxdDJxIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.YIr9CnbuLLo52fz7gjKCGDZnLGsMeH2N2nEzzRBIfoiOGHLMkQiLmH1WJ0806Ou8H6rouXAjKiKkiMcNfbsVj4H5exzCPLxSons3veAosP3b3338MJ8LD73A2pVfjmJTNDQFFuu7ntq9Mc6vkgiwiXyWpF9VfyD9lXnwTeOma8EUohtQ6g_p0k5fN20pYoi57TimVvCTZBatNv7cy6J5M6LrzvprZ0TvvRSwUEou8dW1smPN90s4qx3ld6k4BmOwehrj-OYY9dMcK7GeqK54blrWK0hWQ-PzJINV5c29A0TvDYg47SyGOrDsTwEcP94yceWvCwLqiGgHMGvTmkQ_Fw",
        typeof(Program));


builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();
builder.Services.AddScoped<IProductDAL, ProductDAL>();

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