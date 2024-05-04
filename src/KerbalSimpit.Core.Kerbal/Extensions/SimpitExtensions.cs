using KerbalSimpit.Core.Kerbal.Constants;
using KerbalSimpit.Core.Kerbal.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Kerbal.Extensions
{
    public static class SimpitExtensions
    {
        public static Simpit RegisterKerbal(this Simpit simpit)
        {
            return simpit.RegisterKerbalMessages();
        }

        private static Simpit RegisterKerbalMessages(this Simpit simpit)
        {
            simpit.Messages.RegisterOutogingType<SceneChange>(MessageTypeIds.Outgoing.SceneChange);
            return simpit;
        }
    }
}
