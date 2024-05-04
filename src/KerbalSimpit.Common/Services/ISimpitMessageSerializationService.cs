using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Common.Services
{
    public interface ISimpitMessageSerializationService
    {
        void Register<T>(byte id)
            where T : ISimpitMessage;

        void Serialize(SimpitStream stream, ISimpitMessage message);
        ISimpitMessage Deserialize(SimpitStream stream);
    }
}
