using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;
using TryChannels.ChannelMessages;

namespace TryChannels
{
    internal class Consumer : BackgroundService
    {
        private readonly ChannelService<UserChanged> channel;
        private readonly ILogger<Consumer> logger;

        public Consumer(ChannelService<UserChanged> channel, ILogger<Consumer> logger)
        {
            this.channel = channel;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            while(!stoppingToken.IsCancellationRequested)
            {
                await timer.WaitForNextTickAsync(stoppingToken);

                var messages = channel.GetAll(stoppingToken).ToList();

                var distinctMessages = messages.Distinct();
                logger.LogInformation($"Consuming {messages.Count()} messages compacted to {distinctMessages.Count()}");
                //logger.LogInformation($"all: {string.Join(",", distinctMessages)}");

                foreach( var msg in distinctMessages )
                {
                    await handleMessage(msg);
                }
            }
        }

        protected async Task handleMessage(UserChanged msg)
        {
            await Task.Delay(2000);
            //logger.LogInformation(msg.ToString());
        }
    }
}
