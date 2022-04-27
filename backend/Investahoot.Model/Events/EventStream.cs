using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investahoot.Model.Events
{
    public class EventStream
    {
        private readonly ConcurrentQueue<GameEvent> _events = new();
        public GameEvent LastEvent { get; private set; } = new LobbyEvent(Enumerable.Empty<string>());

        public void PublishEvent(GameEvent e) {
            _events.Enqueue(e);
        }

        public async Task<GameEvent> WaitForEvent(CancellationToken cancellationToken)
        {
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (_events.TryDequeue(out var e)) {
                    LastEvent = e;
                    return e;
                }

                await Task.Delay(100);
            }
        }
    }
}
