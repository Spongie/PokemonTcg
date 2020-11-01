using NetworkingCore;
using System;
using System.Windows;
using System.Windows.Controls;
using TCGCards;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for AddEffectWindow.xaml
    /// </summary>
    public partial class AddAttackWindow : Window
    {
        public AddAttackWindow()
        {
            InitializeComponent();

            var availableEffects = TypeLoader.GetLoadedTypesAssignableFrom<Attack>();
            
            DataContext = availableEffects;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectedAttack = (Attack)Activator.CreateInstance((Type)((Button)sender).DataContext);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public Attack SelectedAttack { get; set; }
    }
}
