using AnagramSolver.BusinessLogic;
using AnagramSolver.Cli;
using AnagramSolver.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = new ConfigurationBuilder();
BuildConfig(builder);
builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<IWordRepository, WordRepository>();
        services.AddTransient<IAnagramSolver, AnagramSolver.BusinessLogic.AnagramSolver>();
        services.AddTransient<Settings>();
        services.AddTransient<UI>();
    })
    .Build();

var anagramSolver = ActivatorUtilities.CreateInstance<AnagramSolver.BusinessLogic.AnagramSolver>(host.Services);
var appUI = ActivatorUtilities.CreateInstance<UI>(host.Services);

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
}


