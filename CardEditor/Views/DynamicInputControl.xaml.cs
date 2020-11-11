using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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
            try
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

                    var panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;
                    panel.MaxHeight = 30;
                    panel.Children.Add(new Label { Content = dynamicInput.DisplayName, MinWidth = 200 });
                    Control input;

                    switch (dynamicInput.InputType)
                    {
                        case InputControl.Text:
                            input = new TextBox() { MinWidth = 100 };
                            input.SetBinding(TextBox.TextProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                            break;
                        case InputControl.Boolean:
                            input = new CheckBox();
                            input.SetBinding(CheckBox.IsCheckedProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                            break;
                        case InputControl.Dropdown:
                            var comboBox = new ComboBox() { MinWidth = 100 };
                            foreach (var value in Enum.GetValues(dynamicInput.EnumType))
                            {
                                comboBox.Items.Add(value);
                            }
                            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                            input = comboBox;
                            break;
                        case InputControl.Grid:
                            var grid = new DataGrid
                            {
                                CanUserAddRows = true,
                                CanUserDeleteRows = true,
                                AutoGenerateColumns = true,
                                MinHeight = 100,
                                MinWidth = 200
                            };
                            grid.SetBinding(DataGrid.ItemsSourceProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                            input = grid;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    panel.Children.Add(input);

                    targetControl.Children.Add(panel);
                }
            }
            catch
            {
                MessageBox.Show("Some error generating inputs, re-select attack");
            }
        }
    }
}
