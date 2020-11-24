using CardEditor.Views;
using Entities;
using System.Linq;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class DamageAllOfSameType : Attack
    {
        private int damageToBench;
        private bool hitsYourBench;
        private bool hitsOpponentBench;

        [DynamicInput("Hits Opponent bench?", InputControl.Boolean)]
        public bool HitsOpponentBench
        {
            get { return hitsOpponentBench; }
            set
            {
                hitsOpponentBench = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Hits your bench?", InputControl.Boolean)]
        public bool HitsYourBench
        {
            get { return hitsYourBench; }
            set
            {
                hitsYourBench = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Damage to bench")]
        public int DamageToBench
        {
            get { return damageToBench; }
            set
            {
                damageToBench = value;
                FirePropertyChanged();
            }
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (opponent.ActivePokemonCard.Type == EnergyTypes.Colorless)
            {
                return;
            }

            if (HitsOpponentBench)
            {
                DealDamageToPlayersBench(opponent, opponent.ActivePokemonCard.Type, game);
            }

            if (hitsYourBench)
            {
                DealDamageToPlayersBench(owner, opponent.ActivePokemonCard.Type, game);
            }

            base.ProcessEffects(game, owner, opponent);
        }

        private void DealDamageToPlayersBench(Player target, EnergyTypes targetType, GameField game)
        {
            foreach (var pokemon in target.BenchedPokemon.Where(x => x.Type == targetType))
            {
                pokemon.DealDamage(damageToBench, game, game.ActivePlayer.ActivePokemonCard);
            }
        }
    }
}
