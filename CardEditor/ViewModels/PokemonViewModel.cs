using CardEditor.Views;
using Entities.Effects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CardEditor.ViewModels
{
    public class PokemonViewModel : DataModel
    {
		private PokemonCard card;
		private Attack selectedAttack;
		private Effect selectedEffect;

		public PokemonViewModel() :this(new PokemonCard())
		{

		}

		public PokemonViewModel(PokemonCard pokemon)
		{
			Card = pokemon;
			AddAttackCommand = new RelayCommand(CanAddAttack, AddAttack);
			AddEffectCommand = new RelayCommand(CanAddEffect, AddEffect);
			RemoveEffectCommand = new RelayCommand(HasEffectSelected, DeleteEffect);
		}

		private void DeleteEffect(object obj)
		{
			SelectedAttack.Effects.Remove(SelectedEffect);
			SelectedEffect = null;
		}

		private void AddEffect(object obj)
		{
			//Select effect
			var dialog = new AddEffectWindow();

			if (dialog.ShowDialog().Value)
			{
				SelectedAttack.Effects.Add(dialog.SelectedEffect);
			}
		}

		private bool HasEffectSelected(object obj)
		{
			return SelectedEffect != null;
		}

		private bool CanAddAttack(object obj) => true;

		private bool CanAddEffect(object obj)
		{
			return SelectedAttack != null;
		}

		private void AddAttack(object obj)
		{
			if (card.Attacks == null)
			{
				card.Attacks = new ObservableCollection<Attack>();
			}

			card.Attacks.Add(new Attack() { Name = "New attack" });
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
		
		public Effect SelectedEffect
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
		public ICommand AddEffectCommand { get; set; }
		public ICommand RemoveEffectCommand { get; set; }
	}
}
