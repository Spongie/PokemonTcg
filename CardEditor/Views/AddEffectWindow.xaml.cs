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

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for AddEffectWindow.xaml
    /// </summary>
    public partial class AddEffectWindow : Window
    {
        public AddEffectWindow()
        {
            InitializeComponent();

            var availableEffects = TypeLoader.GetLoadedTypesAssignableFrom<IEffect>();

            DataContext = availableEffects;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectedEffect = (IEffect)Activator.CreateInstance((Type)((Button)sender).DataContext);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public IEffect SelectedEffect { get; set; }
    }
}
