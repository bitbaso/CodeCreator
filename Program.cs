using CodeCreator;
using CodeCreator.Manager;
using CodeCreator.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;

IHost _host = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
{
    IConfiguration configuration = new ConfigurationBuilder()
       .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
       .Build();

    services.AddTransient(x => new MySqlConnection(configuration.GetConnectionString("Default")));
    services.AddTransient<DatabaseQuery>();
    services.AddTransient<TemplatesQuery>();
    services.AddSingleton<DatabaseSearchManager>();
    services.AddSingleton<DataTransformerManager>();
    services.AddSingleton<CodeTemplateManager>();
    services.AddSingleton<FileCreatorManager>();
    services.AddTransient<Application>();
}).Build();

var app = _host.Services.GetRequiredService<Application>();

await app.Run();