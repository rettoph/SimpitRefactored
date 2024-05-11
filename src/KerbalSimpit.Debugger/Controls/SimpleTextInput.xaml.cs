using System.Windows.Controls;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for SimpleTextInput.xaml
    /// </summary>
    public partial class SimpleTextInput : UserControl
    {
        public SimpleTextInput(string label)
        {
            InitializeComponent();

            this.Label.Content = label;
        }
    }
}
