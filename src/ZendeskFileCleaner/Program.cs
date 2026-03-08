using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZendeskFileCleaner;
using ZendeskFileCleaner.CommandLine;
using ZendeskFileCleaner.Zendesk;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<CommandLineInterface>();
builder.Services.AddSingleton<IZendeskClient, ZendeskClient>();
builder.Services.AddTransient<ITicketDirectoryCleaner, TicketDirectoryCleaner>();
using IHost host = builder.Build();

CommandLineInterface cli = host.Services.GetRequiredService<CommandLineInterface>();
RootCommand rootCommand = cli.CreateRootCommand();

return await rootCommand.Parse(args).InvokeAsync();