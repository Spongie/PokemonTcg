using CardEditor.Views;
using Entities.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TCGCards;
using TCGCards.TrainerEffects;

namespace CardEditor.ViewModels
{
    public class PokemonViewModel : DataModel
    {
		private PokemonCard card;
		private Attack selectedAttack;
		private IEffect selectedEffect;
		private IEffect selectedAbilityEffect;

		public PokemonViewModel() :this(new PokemonCard())
		{

		}

		public PokemonViewModel(PokemonCard pokemon)
		{
			Card = pokemon;
			AddAttackCommand = new RelayCommand(CanAddAttack, AddAttack);
			SetAbilityCommand = new RelayCommand(CanAddAttack, SetAbility);
			AddEffectCommand = new RelayCommand(CanAddEffect, AddEffect);
			AddAbilityEffectCommand = new RelayCommand(CanAddAbilityEffect, AddAbilityEffect);
			DeleteSelectedAttack = new RelayCommand(CanAddAttack, DeleteAttack);
			DeleteSelectedEffect = new RelayCommand(CanAddAttack, DeleteEffect);
			MoveEffectUpCommand = new RelayCommand(CanMoveEffectUp, MoveEffectUp);
			MoveEffectDownCommand = new RelayCommand(CanMoveEffectDown, MoveEffectDown);
		}

        private void MoveEffectUp(object obj)
        {
			var effect = (IEffect)obj;
			var index = SelectedAttack.Effects.IndexOf(effect);

			SelectedAttack.Effects.Move(index, index - 1);
		}

        private bool CanMoveEffectUp(object obj)
        {
			if (SelectedAttack == null)
            {
				return false;
            }

			var index = SelectedAttack.Effects.IndexOf((IEffect)obj);

			return index > 0;
        }

		private void MoveEffectDown(object obj)
		{
			var effect = (IEffect)obj;
			var index = SelectedAttack.Effects.IndexOf(effect);

			SelectedAttack.Effects.Move(index, index + 1);
		}

		private bool CanMoveEffectDown(object obj)
		{
			if (SelectedAttack == null)
			{
				return false;
			}

			var index = SelectedAttack.Effects.IndexOf((IEffect)obj);

			return index < SelectedAttack.Effects.Count - 1;
		}

		private void DeleteEffect(object obj)
        {
			SelectedAttack.Effects.Remove((IEffect)obj);
			SelectedEffect = null;
        }

        private void DeleteAttack(object obj)
        {
			Card.Attacks.Remove((Attack)obj);
			SelectedAttack = null;
        }

        private void AddAbilityEffect(object obj)
        {
			var window = new AddTrainerEffectWindow();

			if (window.ShowDialog().Value)
			{
				card.Ability.Effects.Add(window.SelectedEffect);
				SelectedAbilityEffect = card.Ability.Effects.Last();
			}
		}

        private bool CanAddAbilityEffect(object obj)
        {
			return card.Ability != null;
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

        public IEffect SelectedAbilityEffect
		{
            get { return selectedAbilityEffect; }
            set
            {
                selectedAbilityEffect = value;
                FirePropertyChanged();
            }
        }


        public ICommand AddAttackCommand { get; set; }
        public ICommand SetAbilityCommand { get; set; }
        public ICommand AddAbilityEffectCommand { get; set; }
        public ICommand AddEffectCommand { get; set; }
        public ICommand DeleteSelectedAttack { get; set; }
        public ICommand DeleteSelectedEffect { get; set; }
        public ICommand MoveEffectUpCommand { get; set; }
        public ICommand MoveEffectDownCommand { get; set; }
    }
}
