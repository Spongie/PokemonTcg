﻿using CardEditor.Views;
using Entities.Models;
using NetworkingCore;
using System.Collections.ObjectModel;
using System.Linq;
using TCGCards.TrainerEffects;

namespace TCGCards.Core
{
    public class Ability : DataModel, IEntity
    {
        public const int UNTIL_YOUR_NEXT_TURN = 2;
        protected Card target;
        private string name;
        private string description;
        private int usages = 9999;
        private TriggerType triggerType;
        private ObservableCollection<IEffect> effects = new ObservableCollection<IEffect>();
        private IEffect selectedAbilityEffect;

        public Ability():this(null)
        {

        }

        public Ability(PokemonCard pokemonOwner)
        {
            PokemonOwner = pokemonOwner;
            Id = NetworkId.Generate();
        }

        protected virtual void Activate(Player owner, Player opponent, int damageTaken, GameField game) { }

        private int turnDuration = 9999;

        [DynamicInput("Turn duration")]
        public int TurnDuration
        {
            get { return turnDuration; }
            set
            {
                turnDuration = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Trigger type", InputControl.Dropdown, typeof(TriggerType))]
        public TriggerType TriggerType
        {
            get { return triggerType; }
            set
            {
                triggerType = value;
                FirePropertyChanged();
            }
        }

        public PokemonCard PokemonOwner { get; set; }

        public string AbilityType
        {
            get
            {
                return GetType().Name;
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                FirePropertyChanged();
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                FirePropertyChanged();
            }
        }

        public int Usages
        {
            get { return usages; }
            set
            {
                usages = value;
                FirePropertyChanged();
            }
        }

        public ObservableCollection<IEffect> Effects
        {
            get { return effects; }
            set
            {
                effects = value;
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

        public NetworkId Id { get; set; }
        public int UsedTimes { get; set; }
        public Card Source { get; set; }
        public bool IsBuff { get; set; }

        public void Trigger(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (CanActivate(game, owner, opponent))
            {
                UsedTimes++;
                Activate(owner, opponent, damageTaken, game);
                foreach (var effect in Effects)
                {
                    effect.Process(game, owner, opponent, PokemonOwner);
                }
            }
            else
            {
                game.GameLog.AddMessage(Name + " did not activate because of something");
            }
        }

        public virtual bool CanActivate(GameField game, Player caster, Player opponent)
        {
            return !PokemonOwner.AbilityDisabled 
                && !PokemonOwner.IsAsleep 
                && !PokemonOwner.IsConfused 
                && !PokemonOwner.IsParalyzed 
                && UsedTimes < Usages 
                && Effects.All(effect => effect.CanCast(game, caster, opponent));
        }

        public virtual void SetTarget(Card target)
        {
            this.target = target;
        }

        public virtual void EndTurn() 
        {
            TurnDuration--;
        }

        public void OnDestroyed(GameField game)
        {
            if (Source != null)
            {
                PokemonOwner.Owner.DiscardPile.Add(Source);
                Source = null;
            }
        }
    }
}
