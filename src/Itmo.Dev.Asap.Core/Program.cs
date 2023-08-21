using Itmo.Dev.Asap.Core.Application.Extensions;
using Itmo.Dev.Asap.Core.Application.Handlers.Extensions;
using Itmo.Dev.Asap.Core.DataAccess.Extensions;
using Itmo.Dev.Asap.Core.Presentation.Grpc.Extensions;
using Itmo.Dev.Asap.Core.Presentation.Kafka.Extensions;
using Itmo.Dev.Platform.Logging.Extensions;
using Itmo.Dev.Platform.Postgres.Models;
using Itmo.Dev.Platform.YandexCloud.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
await builder.AddYandexCloudConfigurationAsync();

IConfigurationSection postgresSection = builder.Configuration
    .GetSection("Infrastructure:DataAccess:PostgresConfiguration");

PostgresConnectionConfiguration? postgresConfiguration = postgresSection.Get<PostgresConnectionConfiguration>();

if (postgresConfiguration is null)
    throw new InvalidOperationException("Postgres is not configured");

builder.Services
    .AddApplicationConfiguration()
    .AddHandlers()
    .AddDataAccess(o => o
        .UseNpgsql(postgresConfiguration.ToConnectionString())
        .UseLoggerFactory(LoggerFactory.Create(x => x.AddSerilog().SetMinimumLevel(LogLevel.Trace))))
    .AddGrpcPresentation()
    .AddKafkaPresentation(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.AddPlatformSentry();
builder.Host.AddPlatformSerilog(builder.Configuration);

WebApplication app = builder.Build();

await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    await scope.UseDatabaseContext();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseRouting();
app.UsePlatformSentryTracing(builder.Configuration);

app.UseGrpcPresentation();

await app.RunAsync();