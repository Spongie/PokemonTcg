using NetworkingCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
