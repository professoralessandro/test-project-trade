#region REFERENCES
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;
using Test.Trade.Application.Helper.Static.Settings;
using Test.Trade.Application.Helper.Static.Serilog;
using Test.Trade.Application.Helper.Static.Settings.Jtw;
using Test.Trade.Application.Helper.Static.Generic;
using Test.Trade.Infra.Mappers;
using Test.Trade.Infra.Arquiteture.ServicesInjection;
using Test.Trade.Domain.Context.SQLServer;
#endregion REFERENCES

#region TRY CATCH
try
{
    var builder = WebApplication.CreateBuilder(args);

    #region INTERNAL RUNTIMES
    var config = builder.Configuration;
    var env = builder.Environment;
    RumtimeSettings.ApiName = config.GetValue<string>("ApiConfiguration:ApiName");
    RumtimeSettings.ApiVersion = config.GetValue<string>("ApiConfiguration:Version");
    RumtimeSettings.ApiEnvironment = env.EnvironmentName;

    #region RUNTIMEVARIABLES
    #region CONNECTION STRING
    RumtimeSettings.ConnectionString = config.GetValue<string>("ConnectionString");
    RumtimeSettings.ConnectionStringPostgre = config.GetValue<string>("ConnectionStringPostgre");
    #endregion CONNECTION STRING

    #region API SECRET
    JwtRuntimeConfig.Secret = config.GetValue<string>("JwtConfig:Secret");
    JwtRuntimeConfig.RefreshTokenExpiryTimeInDay = config.GetValue<int>("JwtConfig:RefreshTokenExpiryTimeInDay");
    JwtRuntimeConfig.ExpiresInHour = config.GetValue<int>("JwtConfig:ExpiresInHour");
    JwtRuntimeConfig.Issuer = config.GetValue<string>("JwtConfig:Issuer");
    JwtRuntimeConfig.Audience = config.GetValue<string>("JwtConfig:Audience");
    #endregion API SECRET

    #region API POLICY
    RumtimeSettings.ApiPolicy = config.GetValue<string>("ApiPolicy");
    #endregion API POLICY

    #region TOKEN CONFIGURATION
    #endregion TOKEN CONFIGURATION
    #endregion
    #endregion RUNTIMEVARIABLES

    #region API CONTROLLERS ENDPOINT CONFIGURATION
    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    #endregion API CONTROLLERS ENDPOINT CONFIGURATION

    #region SWAGGER
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "App Test Trade",
                Version = "v1"
            }
        );
        options.SwaggerDoc("v2",
            new OpenApiInfo
            {
                Title = "App Test Trade",
                Version = "v2"
            }
        );
        // options.AddSecurityDefinition("Bearer",
        //    new OpenApiSecurityScheme
        //    {
        //        Description = "Enter 'Bearer [space] and then your token in text input below' ",
        //        Name = "Authorization",
        //        In = ParameterLocation.Header,
        //        Type = SecuritySchemeType.ApiKey,
        //        Scheme = "Bearer"
        //    }
        //);

        // options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        // {
        //     {
        //         new OpenApiSecurityScheme
        //         {
        //             Reference = new OpenApiReference
        //             {
        //                 Type = ReferenceType.SecurityScheme,
        //                 Id = "Bearer"
        //             },
        //             Scheme = "oauth2",
        //             Name = "Bearer",
        //             In = ParameterLocation.Header,
        //         },
        //         new List<string>()
        //     }
        // });
    });

    builder.Services.AddCors(option =>
    {
        option.AddPolicy(RumtimeSettings.ApiPolicy, builder =>
        {
            builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
    });
    #endregion SWAGGER

    #region SERILOG
    builder.Configuration.AddSerilogApi();
    // builder.Host.UseSerilog(Log.Logger);
    builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(LogEventLevel.Debug)
    .WriteTo.File("./SerilogStorage/log.txt",
        LogEventLevel.Warning,
        rollingInterval: RollingInterval.Day));
    #endregion

    #region ENTITY FRAMEWORK
    builder.Services.AddEntityFrameworkSqlServer().AddDbContext<AppDbContext>(a => a.UseSqlServer(RumtimeSettings.ConnectionString));
    #endregion ENTITY FRAMEWORK

    #region JWT CONFIGURATION
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtRuntimeConfig.Secret)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };
    });
    #endregion JWT CONFIGURATION

    #region USE SERVICES
    // SERVICES
    builder.Services.AddServices();
    #endregion

    #region MAPPERS
    // MAPPER
    builder.Services.AddAutoMapper(typeof(MapperDtoToEntity), typeof(MapperEntityToDto));
    #endregion

    #region APP BUILD
    var app = builder.Build();
    #endregion

    #region CONFIGURE APP HTTPS REQUESTS METHOD
    #region DEVELOPMENT ENVRONMENT VALIDAION
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    else
    {
        app.UseHttpsRedirection();
    }
    #endregion DEVELOPMENT ENVRONMENT VALIDAION

    #region API USE CONFIGURATION
    app.UseSerilogRequestLogging();

    //app.UseAuthentication();
    //app.UseAuthorization();
    #region CONFIGURARION CORS
    // app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
    app.UseCors(RumtimeSettings.ApiPolicy);
    #endregion
    #endregion API USE CONFIGURATION

    app.MapControllers();

    app.Run();
    #endregion CONFIGURE APP HTTPS REQUESTS METHOD
}
catch (Exception ex)
{
    // SERILOG REQUEST LOG
    Log.Fatal(ex, UtilHelper.FormatLogInformationMessage(message: "Critical error => Host terminated unexpectedly", userId: Guid.Parse("d2a833de-5bb4-4931-a3c2-133c8994072a"), isHeaderOrFooter: true));
}
finally
{
    // SERILOG REQUEST LOG
    Log.Information(UtilHelper.FormatLogInformationMessage(message: "Critical error => Server Shutting down...", userId: Guid.Parse("d2a833de-5bb4-4931-a3c2-133c8994072a")));
    Log.CloseAndFlush();
}
#endregion TRY CATCH