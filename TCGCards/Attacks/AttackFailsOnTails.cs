using System.Linq;
using CardEditor.Views;
using Entities;
using TCGCards.Core;

namespace TCGCards.Attacks
{
    public class AttackFailsOnTails : Attack
    {
        private bool isOneTime;
        private StatusEffect effect = StatusEffect.None;
        private int selfDamage;
        private string extraIfPokemonOnTeam;
        private int extraDamageName;

        [DynamicInput("Applies status effect?", InputControl.Dropdown, typeof(StatusEffect))]
        public StatusEffect StatusEffect
        {
            get { return effect; }
            set
            {
                effect = value;
                FirePropertyChanged();
            }
        }


        [DynamicInput("Fails forever?", InputControl.Boolean)]
        public bool IsOneTimeUse
        {
            get { return isOneTime; }
            set 
            { 
                isOneTime = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Self damage on fail")]
        public int SelfDamage
        {
            get { return selfDamage; }
            set
            {
                selfDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra damage if this Pokémon on team")]
        public string ExtraIfPokemonOnTeam
        {
            get { return extraIfPokemonOnTeam; }
            set
            {
                extraIfPokemonOnTeam = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Extra damage if Poémon exists")]
        public int ExtraDamageForName
        {
            get { return extraDamageName; }
            set
            {
                extraDamageName = value;
                FirePropertyChanged();
            }
        }

        public override Damage GetDamage(Player owner, Player opponent, GameField game)
        {
            if (game.FlipCoins(1) == 0)
            {
                if (isOneTime)
                {
                    foreverDisabled = true;
                }

                if (SelfDamage > 0)
                {
                    owner.ActivePokemonCard.DealDamage(SelfDamage, game, owner.ActivePokemonCard, false);
                }

                game?.GameLog.AddMessage("The attack did nothing");
                return 0;
            }

            if (StatusEffect != StatusEffect.None)
            {
                opponent.ActivePokemonCard.ApplyStatusEffect(StatusEffect, game);
            }

            int extra = 0;

            if (!string.IsNullOrEmpty(ExtraIfPokemonOnTeam) && owner.BenchedPokemon.ValidPokemonCards.Any(x => x.Name == ExtraIfPokemonOnTeam))
            {
                extra = ExtraDamageForName;    
            }

            return base.GetDamage(owner, opponent, game).NormalDamage + extra;
        }
    }
}
