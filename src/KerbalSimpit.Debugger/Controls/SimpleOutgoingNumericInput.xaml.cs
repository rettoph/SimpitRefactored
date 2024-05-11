using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for SimpleTextSubscriberControl.xaml
    /// </summary>
    public abstract partial class SimpleOutgoingNumericInput : UserControl
    {
        public SimpleOutgoingNumericInput(string title)
        {
            InitializeComponent();

            this.Title.Content = title;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9\\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
