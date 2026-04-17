using Data;
using Data.Users;
using Workflow.Core.Turnstile;
using Workflow.Core.Users;

var builder = WebApplication.CreateBuilder(args);

var turnstileSecretKey = builder.Configuration["Turnstile:SecretKey"] ?? throw new InvalidOperationException("Turnstile secret key not found.");
var turnstileSiteKey = builder.Configuration["Turnstile:SiteKey"]  ?? throw new InvalidOperationException("Turnstile site key not found.");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddSingleton<IDbConnectionFactory>(new DbConnectionFactory(connectionString));
builder.Services.AddHostedService<DbHealthCheckService>();

builder.Services.AddHttpClient<ITurnstileService, TurnstileService>((httpClient) => new TurnstileService(httpClient, turnstileSecretKey, turnstileSiteKey));
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IUserService, UserService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();