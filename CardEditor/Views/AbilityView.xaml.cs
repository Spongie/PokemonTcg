using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TCGCards.Core;

namespace CardEditor.Views
{
    /// <summary>
    /// Interaction logic for AbilityView.xaml
    /// </summary>
    public partial class AbilityView : UserControl
    {
        public AbilityView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ability = (Ability)DataContext;
            var window = new AddTrainerEffectWindow();

            if (window.ShowDialog().Value)
            {
                ability.Effects.Add(window.SelectedEffect);
                ability.SelectedAbilityEffect = ability.Effects.Last();
            }
        }
    }
}
