using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Kerbal.Messages
{
    public struct SceneChange : ISimpitMessageContent
    {
        public enum SceneChangeTypeEnum
        {
            Flight = 0x0,
            NotFlight = 0x1
        }

        public SceneChangeTypeEnum Type;
    }
}
