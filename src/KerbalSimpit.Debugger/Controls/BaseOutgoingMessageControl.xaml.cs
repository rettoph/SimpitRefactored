using System.Reflection;
using System.Windows.Controls;

namespace KerbalSimpit.Debugger.Controls
{
    /// <summary>
    /// Interaction logic for BaseOutgoingMessageControl.xaml
    /// </summary>
    public abstract partial class BaseOutgoingMessageControl : UserControl
    {
        private object _instance;

        public BaseOutgoingMessageControl(Type type)
        {
            _instance = Activator.CreateInstance(type) ?? throw new InvalidOperationException();

            InitializeComponent();

            this.Title.Content = type.Name;

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                SimpleTextInput input = new SimpleTextInput(property.Name);
                this.InputContainer.Children.Add(input);

                if (property.PropertyType == typeof(float))
                {
                    input.Value.TextChanged += (s, e) =>
                    {
                        if (float.TryParse(input.Value.Text, out float value))
                        {
                            property.SetValue(_instance, value);
                            this.OnInstanceUpdated(_instance);
                        }
                    };
                }

                if (property.PropertyType == typeof(int))
                {
                    input.Value.TextChanged += (s, e) =>
                    {
                        if (int.TryParse(input.Value.Text, out int value))
                        {
                            property.SetValue(_instance, value);
                            this.OnInstanceUpdated(_instance);
                        }
                    };
                }
            }
        }

        protected abstract void OnInstanceUpdated(object instance);
    }
}
