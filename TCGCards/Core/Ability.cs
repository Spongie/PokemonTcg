using Entities.Models;
using NetworkingCore;

namespace TCGCards.Core
{
    public abstract class Ability : DataModel, IEntity
    {
        protected Card target;
        private string name;
        private string description;
        private int usages = 9999;
        private TriggerType triggerType;

        protected Ability(PokemonCard pokemonOwner)
        {
            PokemonOwner = pokemonOwner;
            Id = NetworkId.Generate();
        }

        protected abstract void Activate(Player owner, Player opponent, int damageTaken, GameField game);

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

        public NetworkId Id { get; set; }
        public int UsedTimes { get; set; }

        public void Trigger(Player owner, Player opponent, int damageTaken, GameField game)
        {
            if (CanActivate())
            {
                UsedTimes++;
                Activate(owner, opponent, damageTaken, game);
            }
            else
            {
                game.GameLog.AddMessage(Name + " did not activate because of something");
            }
        }

        public virtual bool CanActivate()
        {
            return !PokemonOwner.AbilityDisabled && !PokemonOwner.IsAsleep && !PokemonOwner.IsConfused && !PokemonOwner.IsParalyzed && UsedTimes < Usages;
        }

        public virtual void SetTarget(Card target)
        {
            this.target = target;
        }
    }
}
