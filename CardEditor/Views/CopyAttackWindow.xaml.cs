using CardEditor.Models;
using CardEditor.ViewModels;
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
    /// Interaction logic for CopyAttackWindow.xaml
    /// </summary>
    public partial class CopyAttackWindow : Window
    {
        public CopyAttackWindow(CopyAttackViewModel viewModel)
        {
            InitializeComponent();
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
    }
}
