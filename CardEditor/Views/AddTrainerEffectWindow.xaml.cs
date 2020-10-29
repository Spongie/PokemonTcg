using NetworkingCore;
using System;
using System.Windows;
using System.Windows.Controls;
using TCGCards.TrainerEffects;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for AddTrainerEffectWindow.xaml
    /// </summary>
    public partial class AddTrainerEffectWindow : Window
    {
        public AddTrainerEffectWindow()
        {
            InitializeComponent();
            var availableEffects = TypeLoader.GetLoadedTypesAssignableFrom<ITrainerEffect>();

            DataContext = availableEffects;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectedEffect = (ITrainerEffect)Activator.CreateInstance((Type)((Button)sender).DataContext);
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public ITrainerEffect SelectedEffect { get; set; }
    }
}
