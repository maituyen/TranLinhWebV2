
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
string issuer = "AshProgHelpSecretKeymaivantoan339@gmail.com";
string key = "AshProgHelpSecretKeymaivantoan339@gmail.com";
string NAME_POLICY_CORS = "AllowAllOrigin";
builder.Host.ConfigureAppConfiguration((host, config) =>
{
    config.AddJsonFile("appsettings.json");
    //config.AddJsonFile($"appsettings.{host.HostingEnvironment.EnvironmentName}.json");
});
builder.Services
        .AddCors(opt =>
        {
            opt.AddPolicy(NAME_POLICY_CORS,
                builder =>
                    builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(x => true)
            );
        });
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });
builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("Bearer", policy =>
        {
            policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
            policy.RequireAuthenticatedUser();
        });
    });

builder.Services.AddResponseCaching();
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "image/jpeg", "text/css", "application/javascript" });
});

// Add config swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tran Linh Mobile API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = string.Format("JWT Authoration header using  the Bearer scheme. \r\n\r\nEnter 'Bearer' [space] and then your token in text input below \r\n\r\nExample:'Bearer 123456ADEF'"),
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyApi v1"));

}

app.UseStaticFiles();
app.UseRouting();

//app.ConfigureCustomExceptionMiddleware();

app.UseCors(NAME_POLICY_CORS);

app.UseSwagger();

app.UseResponseCaching();
app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tran Linh Mobile API v1");
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapFallbackToPage("/admin/sanpham/_form");
});
app.Run();