using CardEditor.Views;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class DamageManyAttack : Attack
    {
        private int selfDamage;
        private int teamBenchDamage;
        private int enemyBenchDamage;

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
            var source = game?.ActivePlayer.ActivePokemonCard;

            owner.ActivePokemonCard.DealDamage(SelfDamage, game, source, false);
            foreach (var pokemon in opponent.BenchedPokemon)
            {
                pokemon.DealDamage(EnemyBenchDamage, game, source, true);
            }
            foreach (var pokemon in owner.BenchedPokemon)
            {
                pokemon.DealDamage(TeamBenchDamage, game, source, true);
            }

            base.ProcessEffects(game, owner, opponent);
        }
    }
}
