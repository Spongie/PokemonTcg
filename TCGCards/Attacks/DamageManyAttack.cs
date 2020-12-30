using CardEditor.Views;
using TCGCards.Core;
using TCGCards.TrainerEffects.Util;

namespace TCGCards.Attacks
{
    public class DamageManyAttack : Attack
    {
        private int selfDamage;
        private int teamBenchDamage;
        private int enemyBenchDamage;
        private CoinFlipConditional coinFlipConditional = new CoinFlipConditional();

        [DynamicInput("Condition", InputControl.Dynamic)]
        public CoinFlipConditional CoinFlipConditional
        {
            get { return coinFlipConditional; }
            set
            {
                coinFlipConditional = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to your bench")]
        public int TeamBenchDamage
        {
            get { return teamBenchDamage; }
            set
            {
                teamBenchDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to opponents bench")]
        public int EnemyBenchDamage
        {
            get { return enemyBenchDamage; }
            set
            {
                enemyBenchDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Damage to self")]
        public int SelfDamage
        {
            get { return selfDamage; }
            set 
            { 
                selfDamage = value;
                FirePropertyChanged();
            }
        }

        public override void ProcessEffects(GameField game, Player owner, Player opponent)
        {
            if (!CoinFlipConditional.IsOk(game, owner))
            {
                return;
            }

            var source = game?.ActivePlayer.ActivePokemonCard;

            owner.ActivePokemonCard.DealDamage(SelfDamage, game, source, false);
            foreach (var pokemon in opponent.BenchedPokemon.ValidPokemonCards)
            {
                pokemon.DealDamage(EnemyBenchDamage, game, source, true);
            }
            foreach (var pokemon in owner.BenchedPokemon.ValidPokemonCards)
            {
                pokemon.DealDamage(TeamBenchDamage, game, source, true);
            }

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
