using KerbalSimpit.Core;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.KSP.Extensions;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Core.Peers;
using KerbalSimpit.Core.Utilities;
using Microsoft.Extensions.Options;

namespace KerbalSimpit.Debugger.Services
{
    internal class SimpitService : ISimpitMessageConsumer<CustomLog>
    {
        private readonly Simpit _simpit;
        private readonly Task _loop;
        private CancellationToken _cancellationToken;
        private bool _running;

        public SimpitService(Simpit simpit, IOptions<SimpitConfiguration> options)
        {
            _simpit = simpit;
            _loop = new Task(this.Loop);

            _simpit.RegisterKerbal().AddIncomingConsumer<CustomLog>(this);

            foreach (SimpitConfiguration.SerialConfiguration serial in options.Value.Serial)
            {
                _simpit.AddSerialPeer(serial.Name, serial.BaudRate);
            }
        }

        public void Consume(SimpitPeer peer, ISimpitMessage<CustomLog> message)
        {
            _simpit.Logger.LogInformation($"{nameof(CustomLog)} - {message.Content.Flags}:{message.Content.Value}");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            _running = true;
            _loop.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _running = false;

            return Task.CompletedTask;
        }

        private void Loop()
        {
            _simpit.Start();

            while (_cancellationToken.IsCancellationRequested == false && _running == true)
            {
                _simpit.Flush();

                Thread.Sleep(100);
            }

            _simpit.Stop();
        }
    }
}
