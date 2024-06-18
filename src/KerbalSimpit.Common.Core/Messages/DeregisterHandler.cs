using KerbalSimpit.Common.Core;

namespace KerbalSimpit.Core.Messages
{
    public unsafe struct DeregisterHandler : ISimpitMessageData
    {
        public fixed byte MessageTypeIds[Constants.MaximumMessageSize];
    }
}
