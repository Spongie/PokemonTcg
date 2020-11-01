using CardEditor.ViewModels;
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

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.Save();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.Load();
        }
    }
}
