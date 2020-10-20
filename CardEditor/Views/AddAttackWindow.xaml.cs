using Entities.Effects;
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
