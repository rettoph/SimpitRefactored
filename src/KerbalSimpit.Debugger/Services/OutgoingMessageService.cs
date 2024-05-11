using KerbalSimpit.Core;
using KerbalSimpit.Debugger.Controls;

namespace KerbalSimpit.Debugger.Services
{
    internal class OutgoingMessageService<T> : BaseOutgoingMessageControl
        where T : ISimpitMessageData
    {
        public OutgoingMessageService() : base(typeof(T))
        {
        }

        protected override void OnInstanceUpdated(object instance)
        {
            if (instance is T casted)
            {
                MainWindow.Simpit.SetOutgoingData<T>(casted);
            }
        }
    }
}
