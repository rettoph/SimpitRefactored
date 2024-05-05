using KerbalSimpit.Core;
using KerbalSimpit.Core.Kerbal.Messages;

namespace KerbalSimpit.Debugger.Providers
{
    public class DebugSceneChangeProvider
    {
        private readonly Simpit _simpit;
        private Environment.SceneChange _data;

        public Environment.SceneChange.SceneChangeTypeEnum Type
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
