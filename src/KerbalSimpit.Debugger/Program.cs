using KerbalSimpit.Core;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.Messages;
using KerbalSimpit.Debugger.Subscribers;
using KerbalSimpit.Debugger.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KerbalSimpit.Debugger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cancellation = new CancellationTokenSource();

            Simpit simpit = new Simpit(new ConsoleLogger(SimpitLogLevelEnum.Verbose));
            simpit
                .Subscribe(new CustomLogSubscriber(simpit.Logger))
                .AddSerial("COM4", 115200)
                .Start(cancellation.Token);

            while(true)
            {
                simpit.Flush();
                Thread.Sleep(100);
            }
        }
    }
}
