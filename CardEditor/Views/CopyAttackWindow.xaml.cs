using CardEditor.Models;
using CardEditor.ViewModels;
using PokemonTcgSdk.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for CopyAttackWindow.xaml
    /// </summary>
    public partial class CopyAttackWindow : Window
    {
        private ObservableCollection<PokemonAttack> allAttacks;

        public CopyAttackWindow(CopyAttackViewModel viewModel)
        {
            InitializeComponent();
            allAttacks = viewModel.PokemonAttacks;
            DataContext = viewModel.PokemonAttacks;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            SelectedPokemonAttack = (PokemonAttack)((Button)sender).DataContext;
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public PokemonAttack SelectedPokemonAttack { get; set; }

        private void textPokemonName_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataContext = allAttacks.Where(attack => attack.Pokemon.Name.ToLower().Contains(textPokemonName.Text.ToLower()) && attack.Attack.Name.ToLower().Contains(textAttackName.Text.ToLower())).ToList();
        }
    }
}
