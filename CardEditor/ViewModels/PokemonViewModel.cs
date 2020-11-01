using CardEditor.Views;
using Entities.Effects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TCGCards;
using TCGCards.TrainerEffects;

namespace CardEditor.ViewModels
{
    public class PokemonViewModel : DataModel
    {
		private PokemonCard card;
		private Attack selectedAttack;
		private IEffect selectedEffect;

		public PokemonViewModel() :this(new PokemonCard())
		{

		}

		public PokemonViewModel(PokemonCard pokemon)
		{
			Card = pokemon;
			AddAttackCommand = new RelayCommand(CanAddAttack, AddAttack);
			SetAbilityCommand = new RelayCommand(CanAddAttack, SetAbility);
			AddEffectCommand = new RelayCommand(CanAddEffect, AddEffect);
		}

		private void AddEffect(object obj)
        {
			var window = new AddTrainerEffectWindow();

			if (window.ShowDialog().Value)
			{
				SelectedAttack.Effects.Add(window.SelectedEffect);
				SelectedEffect = SelectedAttack.Effects.Last();
			}
		}

        private bool CanAddEffect(object obj)
        {
			return SelectedAttack != null;
        }

        private bool CanAddAttack(object obj) => true;

		private void AddAttack(object obj)
		{
			if (card.Attacks == null)
			{
				card.Attacks = new ObservableCollection<Attack>();
			}

			var attackDialog = new AddAttackWindow();

			if (attackDialog.ShowDialog().Value)
            {
                try
                {
					card.Attacks.Add(attackDialog.SelectedAttack);
					SelectedAttack = card.Attacks.Last();
				}
				catch (Exception e)
                {
					MessageBox.Show("Some error when adding attack lol");
                }
            }
		}

		private void SetAbility(object obj)
        {
			var avilityDialog = new AddAbilityWindow();

			if (avilityDialog.ShowDialog().Value)
			{
				try
				{
					card.Ability = avilityDialog.SelectedAbility;
				}
				catch (Exception e)
				{
					MessageBox.Show("Some error when adding ability lol");
				}
			}
		}

		public PokemonCard Card
		{
			get { return card; }
			set
			{
				card = value;
				FirePropertyChanged();
			}
		}

		public IEffect SelectedEffect
		{
			get { return selectedEffect; }
			set
			{
				selectedEffect = value;
				FirePropertyChanged();
			}
		}


		public Attack SelectedAttack
		{
			get { return selectedAttack; }
			set
			{
				selectedAttack = value;
				FirePropertyChanged();
			}
		}

		public ICommand AddAttackCommand { get; set; }
        public ICommand SetAbilityCommand { get; set; }
        public ICommand AddEffectCommand { get; set; }
	}
}
