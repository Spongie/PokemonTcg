using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for DynamicInputControl.xaml
    /// </summary>
    public partial class DynamicInputControl : UserControl
    {
        public DynamicInputControl()
        {
            InitializeComponent();
            DataContextChanged += DynamicInputControl_DataContextChanged;
        }

        private void DynamicInputControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            targetControl.Children.Clear();

            if (e.NewValue == null)
            {
                return;
            }

            foreach (var property in e.NewValue.GetType().GetProperties())
            {
                var dynamicInput = property.GetCustomAttribute<DynamicInputAttribute>();

                if (dynamicInput == null)
                {
                    continue;
                }

                var panel = new DockPanel();
                panel.Children.Add(new Label { Content = dynamicInput.DisplayName });
                var input = new TextBox();
                input.SetBinding(TextBox.TextProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });

                panel.Children.Add(input);

                targetControl.Children.Add(panel);
            }
        }
    }
}
