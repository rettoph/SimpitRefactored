using System.Windows;
using System.Windows.Controls;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for CheckboxGrid.xaml
    /// </summary>
    public abstract partial class CheckboxGrid : UserControl
    {
        private Dictionary<CheckBox, object> _valueMap;
        private Dictionary<object, CheckBox> _checkboxMap;

        public CheckboxGrid(string label, object[] values)
        {
            _valueMap = new Dictionary<CheckBox, object>();
            _checkboxMap = new Dictionary<object, CheckBox>();

            InitializeComponent();

            this.Label.Content = label;

            StackPanel stack = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 7) };
            this.CheckboxContainer.Children.Add(stack);
            for (int i = 0; i < values.Length; i++)
            {
                if (i % 4 == 0)
                {
                    stack = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 7) };
                    this.CheckboxContainer.Children.Add(stack);
                }

                CheckBox check = new CheckBox() { Content = values[i].ToString(), Margin = new Thickness(0, 0, 7, 0) };
                check.Checked += this.HandleChecked;
                check.Unchecked += this.HandleUnchecked;

                stack.Children.Add(check);
                _valueMap.Add(check, values[i]);
                _checkboxMap.Add(values[i], check);
            }

            stack.Margin = default;
        }


        protected abstract void Checked(object value);
        protected abstract void Unchecked(object value);

        private void HandleChecked(object sender, RoutedEventArgs e)
        {
            this.Checked(_valueMap[(CheckBox)sender]);
        }

        private void HandleUnchecked(object sender, RoutedEventArgs e)
        {
            this.Unchecked(_valueMap[(CheckBox)sender]);
        }

        protected void SetIsChecked(object value, bool isChecked)
        {
            _checkboxMap[value].IsChecked = isChecked;
        }
    }
}
