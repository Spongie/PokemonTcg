using System.Windows;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for ImportAllFromSet.xaml
    /// </summary>
    public partial class ImportAllFromSet : Window
    {
        public ImportAllFromSet()
        {
            InitializeComponent();
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
