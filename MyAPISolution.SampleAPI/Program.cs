using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyAPISolution.SampleAPI.DAL;
using MyAPISolution.SampleAPI.Filters;
using MyAPISolution.SampleAPI.Helpers;
using MyAPISolution.SampleAPI.Models;
using Serilog;
using Serilog.Filters;
using System.Text;

// Bootstrap logger: captures any startup failures before the host/config is built.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting MyAPISolution.SampleAPI");

    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog from appsettings.json (Serilog section), enriched with context.
    // Split output into two files:
    //  - System log: everything except action-log sources (framework/app diagnostics).
    //  - Action log: TransactionLoggingFilter entries + any logger from the Controllers namespace
    //    (so manual ILogger<T> calls inside controller actions also land in the action log).
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        var systemLogPath = context.Configuration["Serilog:SystemLogPath"] ?? "Logs/system-.txt";
        var actionLogPath = context.Configuration["Serilog:ActionLogPath"] ?? "Logs/action-.txt";
        const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}";

        static bool IsActionLogSource(Serilog.Events.LogEvent evt) =>
            Matching.FromSource<TransactionLoggingFilter>()(evt)
            || Matching.FromSource("MyAPISolution.SampleAPI.Controllers")(evt);

        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.FromLogContext()
            .WriteTo.Logger(lc => lc
                .Filter.ByExcluding(IsActionLogSource)
                .WriteTo.Console(outputTemplate: outputTemplate)
                .WriteTo.File(systemLogPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14, outputTemplate: outputTemplate))
            .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(IsActionLogSource)
                .WriteTo.File(actionLogPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14, outputTemplate: outputTemplate));
    });

    // Add services to the container.
    builder.Services.AddControllers(options =>
    {
        options.Filters.Add<TransactionLoggingFilter>();
    }).AddJsonOptions(options =>
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

//add identitiy 
builder.Services.AddIdentity<IdentityUser, IdentityRole>(builder =>
{
    builder.Password.RequireDigit = true;
    builder.Password.RequireLowercase = true;
    builder.Password.RequireUppercase = true;
    builder.Password.RequireNonAlphanumeric = true;
    builder.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<RapidDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();
builder.Services.AddScoped<IProductDAL, ProductDAL>();

builder.Services.AddScoped<IAuthDAL, AuthDAL>();

var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//menambahkan custom middleware
/*app.Run(async (context)=>
{
    await context.Response.WriteAsync("Hello World!");
});*/

app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "MyAPISolution.SampleAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}