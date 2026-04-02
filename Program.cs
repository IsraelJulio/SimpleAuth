using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    options.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        var url = context.RedirectUri + "&prompt=select_account";
        context.Response.Redirect(url);
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

string[] allowedOrigins = new string[]
{
    "http://localhost:4200",
    "https://stg.seusite.com",
    "https://app.seusite.com"
};

bool IsAllowedReturnUrl(string returnUrl)
{
    if (!Uri.TryCreate(returnUrl, UriKind.Absolute, out var uri))
        return false;

    var origin = $"{uri.Scheme}://{uri.Host}" + (uri.IsDefaultPort ? "" : $":{uri.Port}");
    return allowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase);
}

app.MapGet("/auth/google/login", async (HttpContext context) =>
{
    var returnUrl = context.Request.Query["returnUrl"].ToString();

    if (string.IsNullOrWhiteSpace(returnUrl) || !IsAllowedReturnUrl(returnUrl))
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Invalid returnUrl");
        return;
    }

    var properties = new AuthenticationProperties
    {
        RedirectUri = $"/auth/google/callback-success?returnUrl={Uri.EscapeDataString(returnUrl)}"
    };

    await context.ChallengeAsync(GoogleDefaults.AuthenticationScheme, properties);
});

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
