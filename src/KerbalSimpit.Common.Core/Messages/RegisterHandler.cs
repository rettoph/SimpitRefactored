using KerbalSimpit.Common.Core;

namespace KerbalSimpit.Core.Messages
{
    public unsafe struct RegisterHandler : ISimpitMessageData
    {
        public fixed byte MessageTypeIds[Constants.MaximumMessageSize];
    }
}
