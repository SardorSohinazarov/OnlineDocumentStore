using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineDocumentStore.API.Middlewares;
using OnlineDocumentStore.Application;
using OnlineDocumentStore.Application.Models.Auth;
using OnlineDocumentStore.Infrastructure;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddEndpointsApiExplorer();

#region Swaggerdagi Authorize button
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GpsTracker", Version = "v1.0.0" });
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
    c.AddSecurityRequirement(securityRequirement);
});
#endregion

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

#region AuthenticationService
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters = GetTokenValidationParameters(builder.Configuration.GetSection("JWT").Get<JWTConfiguration>());

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = (context) =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        var serializedObject = JsonSerializer.Serialize(new
                        {
                            context.Exception.Message
                        });
                        context.Response.WriteAsync(serializedObject).Wait();
                    }

                    return Task.CompletedTask;
                }
            };
        });
#endregion

var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{*/
app.UseSwagger();
app.UseSwaggerUI();
/*}*/

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


#region ValidationService
static TokenValidationParameters GetTokenValidationParameters(JWTConfiguration configuration)
{
    return new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = configuration.Issuer,
        ValidateAudience = true,
        ValidAudience = configuration.Audience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key)),
        ClockSkew = TimeSpan.Zero,
    };
}
#endregion