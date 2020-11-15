using CardEditor.Views;

namespace TCGCards.Core.Abilities
{
    public class BounceSelfAbility : Ability
    {
        private bool bounceAttached;

        [DynamicInput("Bounce attached cards", InputControl.Boolean)]
        public bool BounceAttachedCards
        {
            get { return bounceAttached; }
            set
            {
                bounceAttached = value;
                FirePropertyChanged();
            }
        }

        public BounceSelfAbility() :this(null)
        {

        }

        public BounceSelfAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Activation;
        }

        public override bool CanActivate()
        {
            return !PokemonOwner.PlayedThisTurn;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            owner.Hand.Add(PokemonOwner);

            foreach (var card in PokemonOwner.AttachedEnergy)
            {
                if (BounceAttachedCards)
                {
                    owner.Hand.Add(PokemonOwner);
                }
                else
                {
                    owner.DiscardPile.Add(card);
                }
            }

            PokemonOwner.AttachedEnergy.Clear();
            PokemonOwner.DamageCounters = 0;

            if (owner.ActivePokemonCard.Id.Equals(PokemonOwner.Id))
            {
                owner.ActivePokemonCard = null;
            }
            else
            {
                owner.BenchedPokemon.Remove(PokemonOwner);
            }
        }
    }
}
