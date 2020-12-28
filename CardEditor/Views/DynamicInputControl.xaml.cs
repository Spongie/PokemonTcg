using CardEditor.ViewModels;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TCGCards.TrainerEffects;

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

                    var panel = new DockPanel();
                    panel.LastChildFill = true;
                    //panel.MaxHeight = 30;
                    if (dynamicInput.InputType != InputControl.Ability)
                    {
                        var label = new Label { Content = dynamicInput.DisplayName, MinWidth = 200 };
                        panel.Children.Add(label);
                        DockPanel.SetDock(label, Dock.Left);
                    }
                    DockPanel.SetDock(panel, Dock.Top);
                    Control input;

                    switch (dynamicInput.InputType)
                    {
                        case InputControl.Text:
                            input = new TextBox() { MinWidth = 100 };
                            input.SetBinding(TextBox.TextProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                            DockPanel.SetDock(input, Dock.Left);
                            break;
                        case InputControl.Boolean:
                            input = new CheckBox();
                            input.SetBinding(CheckBox.IsCheckedProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                            DockPanel.SetDock(input, Dock.Left);
                            break;
                        case InputControl.Dropdown:
                            var comboBox = new ComboBox() { MinWidth = 100 };
                            foreach (var value in Enum.GetValues(dynamicInput.EnumType))
                            {
                                comboBox.Items.Add(value);
                            }
                            comboBox.SetBinding(ComboBox.SelectedItemProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                            input = comboBox;
                            DockPanel.SetDock(input, Dock.Left);
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
                            DockPanel.SetDock(input, Dock.Left);
                            break;
                        case InputControl.Ability:
                            var button = new Button { Content = "Set Ability" };
                            button.Command = new RelayCommand((x) => { return true; }, AddAbility);
                            DockPanel.SetDock(button, Dock.Top);
                            panel.Children.Add(button);
                            var abilityInput = new AbilityView();
                            abilityInput.MinHeight = 320;
                            abilityInput.SetBinding(AbilityView.DataContextProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                            panel.VerticalAlignment = VerticalAlignment.Stretch;
                            DockPanel.SetDock(abilityInput, Dock.Top);
                            input = abilityInput;
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

        private void AddAbility(object obj)
        {
            var avilityDialog = new AddAbilityWindow();

            if (avilityDialog.ShowDialog().Value)
            {
                try
                {
                    if (DataContext is AttachmentEffect)
                    {
                        var effect = (AttachmentEffect)DataContext;
                        effect.Ability = avilityDialog.SelectedAbility;
                    }
                    else if (DataContext is CreateAbilityOnGameEffect)
                    {
                        var effect = (CreateAbilityOnGameEffect)DataContext;
                        effect.Ability = avilityDialog.SelectedAbility;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Some error when adding ability lol");
                }
            }
        }
    }
}
