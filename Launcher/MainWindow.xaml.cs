using System.Windows;

namespace Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller controller;

        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller();
            DataContext = controller;
        }

        private void LaunchClick(object sender, RoutedEventArgs e)
        {
            controller.LaunchGame();
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            controller.UpdateStart();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            controller.Disconnect();
        }
    }
}
