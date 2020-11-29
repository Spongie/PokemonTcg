using CardEditor.ViewModels;
using NetworkingCore;
using System.IO;
using System.Net.Sockets;
using System.Windows;

namespace CardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        public MainWindow()
        {
            viewModel = new MainViewModel();
            InitializeComponent();
            DataContext = viewModel;
        }

        private async void Button_Commit_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.Commit();
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.Save();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.Load();
        }

        private async void Upload_Click(object sender, RoutedEventArgs e)
        {
            NetworkPlayer player = CreateConnection();

            var cardService = new CardService(player);

            var pokemons = await File.ReadAllTextAsync("Data/pokemon.json");
            var sets = await File.ReadAllTextAsync("Data/sets.json");
            var energy = await File.ReadAllTextAsync("Data/energy.json");
            var trainers = await File.ReadAllTextAsync("Data/trainers.json");

            cardService.UpdateCards(pokemons, energy, trainers, sets);

            MessageBox.Show("Data uploaded!");
        }

        private static NetworkPlayer CreateConnection()
        {
            var tcp = new TcpClient();
            tcp.Connect("85.90.244.171", 8080);
            //tcp.Connect("127.0.0.1", 8080);
            var player = new NetworkPlayer(tcp);

            while (player.Id == null) { }

            return player;
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            var player = CreateConnection();
            var service = new InfoService(player);

            var pokemons = service.GetPokemonJson();
            await File.WriteAllTextAsync("Data/pokemon.json", pokemons);

            var sets = service.GetSetsJson();
            await File.WriteAllTextAsync("Data/sets.json", sets);

            var trainers = service.GetTrainerJson();
            await File.WriteAllTextAsync("Data/trainers.json", trainers);

            var energy = service.GetEnergyJson();
            await File.WriteAllTextAsync("Data/energy.json", energy);

            await viewModel.Load();

            MessageBox.Show("Data refreshed!");
        }
    }
}
