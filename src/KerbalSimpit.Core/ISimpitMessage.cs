using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core
{
    public interface ISimpitMessage
    {
        SimpitMessageType Type { get; }
        ISimpitMessageContent Content { get; }
    }

    public interface ISimpitMessage<TContent> : ISimpitMessage
        where TContent : ISimpitMessageContent
    {
        new SimpitMessageType<TContent> Type { get; }
        new TContent Content { get; }
    }
}
