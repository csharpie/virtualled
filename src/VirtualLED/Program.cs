using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using VirtualLED.Components;
using VirtualLED.Hubs;
using VirtualLED.Middlewares;
using VirtualLED.Models;
using VirtualLED.Services;

var logger = Log.Logger = new LoggerConfiguration()
    .CreateLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));
    builder.Services.AddHttpLogging(o => { });

    // Add DI
    builder.Services.AddSingleton<IColorService, LEDColorService>();
    builder.Services.AddScoped<ITokenService, TokenService>();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddSignalR();

    builder.Services.AddHttpContextAccessor();
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();

    if (string.IsNullOrWhiteSpace(jwtSettings.Key))
    {
        Log.Fatal("JwtSettings:Key is not configured");
        throw new InvalidOperationException("JwtSettings:Key is not configured.");
    }

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });
        options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("bearer", document)] = []
        });
    });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"] ?? string.Empty)),
                ClockSkew = TimeSpan.Zero
            };
        });

    builder.Services.AddAuthorization();


    var app = builder.Build();

    app.UseExceptionHandler();

    app.UseHttpLogging();
    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseForwardedHeaders();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.MapHub<LEDColorHub>("/ledcolorhub");

    app.MapControllers();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}
