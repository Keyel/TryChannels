using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TryChannels.ChannelMessages;

namespace TryChannels
{
    internal class Publisher : BackgroundService
    {
        private readonly ChannelService<UserChanged> channel;
        private readonly ILogger<Publisher> logger;

        public Publisher(ChannelService<UserChanged> channel, ILogger<Publisher> logger)
        {
            this.channel = channel;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Random random = new Random();
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMilliseconds(100));
            while(!stoppingToken.IsCancellationRequested)
            {
                await timer.WaitForNextTickAsync();

                var payload = new UserChanged(
                    UserID: random.Next(5)
                );

                //logger.LogInformation(payload.ToString());
                await channel.AddMessageAsync(payload, stoppingToken);
            }

            logger.LogInformation("Closing, cancellation was requested!");
        }
    }
}
