using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace TryChannels
{
    public class ChannelService<T> where T : class
    {
        private readonly Channel<T> _channel;

        public ChannelService()
        {
            _channel = Channel.CreateBounded<T>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.DropWrite,
                SingleReader = true,
                SingleWriter = false,
            });
        }

        public async Task AddMessageAsync(T message, CancellationToken cancellationToken)
        {
            await _channel.Writer.WriteAsync(message, cancellationToken);
        }

        public IAsyncEnumerable<T> GetAllAsync(CancellationToken cancellationToken)
        {
            return _channel.Reader.ReadAllAsync(cancellationToken);
        }
        public List<T> GetAll(CancellationToken cancellationToken)
        {
            List<T> result = new List<T>();
            while( this._channel.Reader.TryRead(out T? msg) )
            {
                result.Add(msg);
            }

            return result;
        }
    }
}
