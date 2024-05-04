using KerbalSimpit.Core;
using KerbalSimpit.Core.Enums;
using KerbalSimpit.Core.Extensions;
using KerbalSimpit.Core.Kerbal.Extensions;
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
            Simpit simpit = new Simpit(new ConsoleLogger(SimpitLogLevelEnum.Verbose));
            simpit
                .RegisterKerbal()
                .RegisterIncomingConsumer(new CustomLogSubscriber(simpit.Logger))
                .RegisterSerial("COM4", 115200)
                .Start();

            while(true)
            {
                simpit.Flush();
                Thread.Sleep(100);
            }
        }
    }
}
