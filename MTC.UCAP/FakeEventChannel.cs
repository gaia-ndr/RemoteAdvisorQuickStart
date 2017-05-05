using System;
using System.Threading.Tasks;
using Microsoft.SfB.PlatformService.SDK.ClientModel;

namespace MTC.UCAP
{
    /// <summary>
    /// Fake event channel used in samples where call back is not required
    /// </summary>
    public class FakeEventChannel : IEventChannel
    {
        public event EventHandler<EventsChannelArgs> HandleIncomingEvents;

        public Task TryStartAsync()
        {
            return Task.CompletedTask;
        }

        public Task TryStopAsync()
        {
            return Task.CompletedTask;
        }
    }
}
