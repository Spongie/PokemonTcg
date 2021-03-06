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
        private bool usableWhileActive = true;
        private bool usableWhileBenched = true;
        private bool usableWhileParalyzed = false;
        private bool usableWhileAsleep = false;
        private bool usableWhileConfused = false;
        private int turnDuration = 9999;
        private bool usableImmediately = true;
        private bool removedWhenBenched;

        public Ability():this(null)
        {

        }

        public Ability(PokemonCard pokemonOwner)
        {
            PokemonOwner = pokemonOwner;
            Id = NetworkId.Generate();
        }

        protected virtual void Activate(Player owner, Player opponent, int damageTaken, GameField game) { }

        [DynamicInput("Usable Immediately", InputControl.Boolean)]
        public bool UsableImmediately
        {
            get { return usableImmediately; }
            set
            {
                usableImmediately = value;
                FirePropertyChanged();
            }
        }


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

        public bool UsableWhileConfused
        {
            get { return usableWhileConfused; }
            set
            {
                usableWhileConfused = value;
                FirePropertyChanged();
            }
        }

        public bool UsableWhileAsleep
        {
            get { return usableWhileAsleep; }
            set
            {
                usableWhileAsleep = value;
                FirePropertyChanged();
            }
        }

        public bool UsableWhileParalyzed
        {
            get { return usableWhileParalyzed; }
            set
            {
                usableWhileParalyzed = value;
                FirePropertyChanged();
            }
        }


        public bool UsableWhileActive
        {
            get { return usableWhileActive; }
            set
            {
                usableWhileActive = value;
                FirePropertyChanged();
            }
        }

        public bool UsableWhileBenched
        {
            get { return usableWhileBenched; }
            set
            {
                usableWhileBenched = value;
                FirePropertyChanged();
            }
        }


        public NetworkId Id { get; set; }
        public int UsedTimes { get; set; }
        public Card Source { get; set; }
        public bool IsBuff { get; set; }

        public bool RemovedWhenBenched
        {
            get { return removedWhenBenched; }
            set
            {
                removedWhenBenched = value;
                FirePropertyChanged();
            }
        }


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
            if (PokemonOwner != null)
            {
                if (!UsableImmediately && PokemonOwner.PlayedThisTurn)
                {
                    game.GameLog.AddMessage($"{Name} cannot be used the same turn {PokemonOwner.Name} is played");
                    return false;
                }
                if (IsBuff)
                {
                    return true;
                }
                if (!usableWhileActive && caster.ActivePokemonCard == PokemonOwner)
                {
                    game.GameLog.AddMessage($"{Name} cannot be used while {PokemonOwner.Name} is Active");
                    return false;
                }
                else if (!usableWhileBenched && caster.BenchedPokemon.Contains(PokemonOwner))
                {
                    game.GameLog.AddMessage($"{Name} cannot be used while {PokemonOwner.Name} is Benched");
                    return false;
                }
                else if (!UsableWhileAsleep && PokemonOwner.IsAsleep)
                {
                    game.GameLog.AddMessage($"{Name} cannot be used while {PokemonOwner.Name} is Asleep");
                    return false;
                }
                else if (!UsableWhileConfused && PokemonOwner.IsConfused)
                {
                    game.GameLog.AddMessage($"{Name} cannot be used while {PokemonOwner.Name} is Confused");
                    return false;
                }
                else if (!UsableWhileParalyzed && PokemonOwner.IsParalyzed)
                {
                    game.GameLog.AddMessage($"{Name} cannot be used while {PokemonOwner.Name} is Paralyzed");
                    return false;
                }
                else if (PokemonOwner.AbilityDisabled)
                {
                    game.GameLog.AddMessage($"{Name} is disabled by something");
                    return false;
                }
            }

            if (UsedTimes >= Usages)
            {
                game.GameLog.AddMessage($"{Name} has been used {UsedTimes} and has a limit of {Usages}");
                return false;
            }

            foreach (var effect in Effects)
            {
                if (!effect.CanCast(game, caster, opponent))
                {
                    game.GameLog.AddMessage($"{effect.EffectType} cannot be used");
                    return false;
                }
            }

            return true;
        }

        public Player GetActivator(Player activePlayer)
        {
            if (PokemonOwner != null)
            {
                return PokemonOwner.Owner;
            }

            return activePlayer;
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
