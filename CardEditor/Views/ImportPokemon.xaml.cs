using System.Windows;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for ImportPokemon.xaml
    /// </summary>
    public partial class ImportPokemon : Window
    {
        public ImportPokemon()
        {
            InitializeComponent();
            urlTextbox.Text = "https://api.pokemontcg.io/v1/cards/";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Url = urlTextbox.Text;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public string Url { get; set; }
    }
}
