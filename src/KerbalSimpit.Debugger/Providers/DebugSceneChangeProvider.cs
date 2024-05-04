using KerbalSimpit.Core;
using KerbalSimpit.Core.Kerbal.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Debugger.Providers
{
    public class DebugSceneChangeProvider
    {
        private readonly Simpit _simpit;
        private SceneChange _data;

        public SceneChange.SceneChangeTypeEnum Type
        {
            get => _data.Type;
            set
            {
                _data.Type = value;
                _simpit.Broadcast(_data);
            }
        }

        public DebugSceneChangeProvider(Simpit simpit)
        {
            _simpit = simpit;
        }
    }
}
