using KerbalSimpit.Debugger.Controls;
using System.Windows.Controls;

namespace KerbalSimpit.Debugger.Services
{
    internal class GenericValueInputService<T> : SimpleOutgoingNumericInput
        where T : notnull
    {
        private T _value;
        private Func<string, T> _deserialize;
        private Action<T> _onChange;


        public GenericValueInputService(string title, T value, Func<string, T> deserialize, Action<T> onChanged) : base(title)
        {
            _value = value;
            _deserialize = deserialize;
            _onChange = onChanged;

            this.Value.Text = value.ToString();
            this.Value.TextChanged += this.HandleValueChanged;
        }

        private void HandleValueChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                T value = _deserialize(this.Value.Text);
                _onChange(value);
            }
            catch (Exception ex)
            {
                // Silence, error.
            }
        }
    }
}
