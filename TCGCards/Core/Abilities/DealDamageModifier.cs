using CardEditor.Views;
using System;

namespace TCGCards.Core.Abilities
{
    public class DealDamageModifier : Ability, IDamageDealtModifier
    {
        private float modifer;
        private bool roundDown;
        private bool onlyIfAnyDamage;
        private bool coinFlip;

        [DynamicInput("Only add damage if any damage", InputControl.Boolean)]
        public bool OnlyIfAnyDamage
        {
            get { return onlyIfAnyDamage; }
            set
            {
                onlyIfAnyDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Round down?", InputControl.Boolean)]
        public bool RoundDown
        {
            get { return roundDown; }
            set
            {
                roundDown = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Damage modifier (0-1 for %)")]
        public float Modifer
        {
            get { return modifer; }
            set
            {
                modifer = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Coin Flip", InputControl.Boolean)]
        public bool CoinFlip
        {
            get { return coinFlip; }
            set
            {
                coinFlip = value;
                FirePropertyChanged();
            }
        }

        public DealDamageModifier() : this(null)
        {

        }

        public DealDamageModifier(PokemonCard pokemonOwner) : base(pokemonOwner)
        {
            TriggerType = TriggerType.DamageDealtModifier;
        }

        protected override void Activate(Player owner, Player opponent, int damageTaken, GameField game)
        {
            foreach (var effect in Effects)
            {
                effect.Process(game, owner, opponent, PokemonOwner);
            }
        }

        public int GetModifiedDamage(int damageDone, GameField game)
        {
            if (CoinFlip && game.FlipCoins(1) == 0)
            {
                Activate(game.ActivePlayer, game.NonActivePlayer, damageDone, game);
                return damageDone;
            }

            if (damageDone == 0 && OnlyIfAnyDamage)
            {
                Activate(game.ActivePlayer, game.NonActivePlayer, damageDone, game);
                return damageDone;
            }

            var damageWithPrevention = Modifer < 1 ? Math.Ceiling(damageDone * Modifer) : damageDone + Modifer;

            if (damageWithPrevention % 5 == 0 && damageWithPrevention % 10 != 0)
            {
                if (RoundDown)
                {
                    damageWithPrevention -= 5;
                }
                else
                {
                    damageWithPrevention += 5;
                }
            }

            Activate(game.ActivePlayer, game.NonActivePlayer, damageDone, game);
            return (int)Math.Max(damageWithPrevention, 0);
        }
    }
}
