using CardEditor.Views;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Core.Abilities
{
    public class AttackStopperAbility : Ability, IAttackStoppingAbility
    {
        public AttackStopperAbility() : this(null)
        {

        }

        public AttackStopperAbility(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.Attacked;
        }

        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();

        [DynamicInput("Coin Flip", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinFlipConditional; }
            set
            {
                coinFlipConditional = value;
                FirePropertyChanged();
            }
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            
        }

        public bool IsStopped(GameField game, PokemonCard attacker, PokemonCard defender)
        {
            return CoinFlipConditional.IsOk(game, PokemonOwner.Owner);
        }
    }
}
