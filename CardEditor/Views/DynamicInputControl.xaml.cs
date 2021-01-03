using CardEditor.ViewModels;
using Entities;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TCGCards.TrainerEffects;
using TCGCards.TrainerEffects.Util;

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

                var valuesetter = e.NewValue.GetType().GetCustomAttribute<ValueSetterAttribute>();
                if (valuesetter != null)
                {
                    CreateValueSetterInput(valuesetter, e.NewValue);
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
                    
                    if (dynamicInput.InputType != InputControl.Ability && dynamicInput.InputType != InputControl.Dynamic)
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
                        case InputControl.Dynamic:
                            {
                                var dynamicGroup = new GroupBox();
                                
                                var control = new DynamicInputControl();
                                control.SetBinding(DynamicInputControl.DataContextProperty, new Binding(property.Name) { Mode = BindingMode.TwoWay });
                                dynamicGroup.Content = control;
                                dynamicGroup.Header = dynamicInput.DisplayName;
                                input = dynamicGroup;
                                break;
                            }
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

        private void CreateValueSetterInput(ValueSetterAttribute valuesetter, object target)
        {
            var panel = new DockPanel();
            panel.LastChildFill = true;

            var propertyCombobox = new ComboBox();
            propertyCombobox.DisplayMemberPath = "PropertyName";
            propertyCombobox.MinWidth = 150;

            var x = target.GetType().GetProperty("SelectedProperty").GetValue(target);

            foreach (var p in valuesetter.TargetType.GetProperties())
            {
                if (p.PropertyType.Namespace.StartsWith("System"))
                {
                    propertyCombobox.Items.Add(new PropertyInfoData(p));
                }
            }


            propertyCombobox.SetBinding(ComboBox.SelectedItemProperty, new Binding("SelectedProperty") { Mode = BindingMode.TwoWay });
            DockPanel.SetDock(propertyCombobox, Dock.Left);

            var textInput = new TextBox();
            textInput.SetBinding(TextBox.TextProperty, new Binding("ValueToSet") { Mode = BindingMode.TwoWay });
            DockPanel.SetDock(textInput, Dock.Left);

            var label = new Label { Content = "Property" };
            DockPanel.SetDock(label, Dock.Left);

            panel.Children.Add(label);
            panel.Children.Add(propertyCombobox);
            panel.Children.Add(textInput);

            targetControl.Children.Add(panel);
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
