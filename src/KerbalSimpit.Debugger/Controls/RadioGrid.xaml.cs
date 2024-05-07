using System.Windows;
using System.Windows.Controls;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for RadioGrid.xaml
    /// </summary>
    public abstract partial class RadioGrid : UserControl
    {
        private Dictionary<RadioButton, object> _valueMap;
        private Dictionary<object, RadioButton> _checkboxMap;

        public RadioGrid(string label, object[] values, int columns)
        {
            _valueMap = new Dictionary<RadioButton, object>();
            _checkboxMap = new Dictionary<object, RadioButton>();

            InitializeComponent();

            this.Label.Content = label;

            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 7) };
            this.RadioContainer.Children.Add(stack);
            for (int i = 0; i < values.Length; i++)
            {
                if (i % columns == 0)
                {
                    stack = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 7) };
                    this.RadioContainer.Children.Add(stack);
                }

                RadioButton radio = new RadioButton() { Content = values[i].ToString(), Margin = new Thickness(0, 0, 7, 0), GroupName = label };
                radio.Checked += this.HandleChecked;
                radio.Unchecked += this.HandleUnchecked;

                stack.Children.Add(radio);
                _valueMap.Add(radio, values[i]);
                _checkboxMap.Add(values[i], radio);
            }

            stack.Margin = default;
        }


        protected abstract void Checked(object value);
        protected abstract void Unchecked(object value);

        private void HandleChecked(object sender, RoutedEventArgs e)
        {
            this.Checked(_valueMap[(RadioButton)sender]);
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            this.Unchecked(_valueMap[(RadioButton)sender]);
        }

        protected void SetIsChecked(object value, bool isChecked)
        {
            _checkboxMap[value].IsChecked = isChecked;
        }
    }
}
