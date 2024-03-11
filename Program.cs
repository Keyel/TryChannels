// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using TryChannels;
using TryChannels.ChannelMessages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.Services
        .AddSingleton(typeof(ChannelService<>))
        .AddHostedService<Publisher>()
        .AddHostedService<Consumer>();
        
var app = builder.Build();
app.Run();

//Console.WriteLine("Hello, World!");
//var serviceProvider = ConfigureServices();

//Console.ReadLine();
//serviceProvider.GetRequiredService<ChannelService<UserChanged>>();
//var publisher = serviceProvider.GetRequiredService<Publisher>();


