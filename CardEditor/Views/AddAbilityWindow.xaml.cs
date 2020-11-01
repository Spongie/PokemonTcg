using NetworkingCore;
using System;
using System.Windows;
using System.Windows.Controls;
using TCGCards.Core;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for AddAbilityWindow.xaml
    /// </summary>
    public partial class AddAbilityWindow : Window
    {
        public AddAbilityWindow()
        {
            InitializeComponent();

            var availableEffects = TypeLoader.GetLoadedTypesAssignableFrom<Ability>();

            DataContext = availableEffects;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectedAbility = (Ability)Activator.CreateInstance((Type)((Button)sender).DataContext);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public Ability SelectedAbility { get; set; }
    }
}
