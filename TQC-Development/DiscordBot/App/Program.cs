﻿using DatabaseAccess.Database;
using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.Interfaces;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Discord Services.
                    services.AddSingleton<DiscordSocketClient, DiscordSocketClient>();
                    services.AddSingleton<DiscordSocketConfig>();

                    // Database Services.
                    services.AddScoped<IDatabase, SqlDatabase>();
                    services.AddScoped<ILogRepository, DatabaseLogRepository>();                    

                    // Utility Services.
                    services.AddScoped<IDataService, DataService>();
                    services.AddScoped<INotifier, NotificationService>();
                    services.AddScoped<IClanApplication, ClanApplicationService>();

                    // Log Services.
                    services.AddScoped<ILogger, LogService>();

                    // Startup Services.
                    services.AddSingleton<IStartup, MinionStartup>();

                    services.Configure<DiscordSocketConfig>(options =>
                    {
                        options.LogLevel = LogSeverity.Info;
                        options.MessageCacheSize = 100;
                        options.ExclusiveBulkDelete = false;
                        options.AlwaysDownloadUsers = true;
                    });
                })
                .Build();

            var startupService = ActivatorUtilities.GetServiceOrCreateInstance<IStartup>(host.Services);

            await startupService.InitBotAsync();
        }

        private static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
