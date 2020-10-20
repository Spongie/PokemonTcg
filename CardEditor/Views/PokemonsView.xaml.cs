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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TCGCards;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for PokemonsView.xaml
    /// </summary>
    public partial class PokemonsView : UserControl
    {
        public PokemonsView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var atk = asd.SelectedItem as Attack;
            //MessageBox.Show(atk.Name);
        }
    }
}
