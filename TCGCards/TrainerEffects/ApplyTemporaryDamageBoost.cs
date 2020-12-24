using CardEditor.Views;
using Entities.Models;
using System.Linq;
using TCGCards.Core;
using TCGCards.Core.SpecialAbilities;

namespace TCGCards.TrainerEffects
{
    public class ApplyTemporaryDamageBoost : DataModel, IEffect
    {
        private string attackName;
        private int newDamage;
        private int turns = 3;

        [DynamicInput("How many turns")]
        public int Turns
        {
            get { return turns; }
            set
            {
                turns = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("New Damage")]
        public int NewDamage
        {
            get { return newDamage; }
            set
            {
                newDamage = value;
                FirePropertyChanged();
            }
        }

        [DynamicInput("Target attack")]
        public string AttackName
        {
            get { return attackName; }
            set
            {
                attackName = value;
                FirePropertyChanged();
            }
        }

        public string EffectType
        {
            get
            {
                return "Apply temporary damage boost";
            }
        }

        public bool CanCast(GameField game, Player caster, Player opponent)
        {
            return true;
        }

        public void OnAttachedTo(PokemonCard attachedTo, bool fromHand, GameField game)
        {
            ApplyModifier(attachedTo);
        }

        private void ApplyModifier(PokemonCard pokemon)
        {
            var attack = pokemon.Attacks.FirstOrDefault(x => x.Name.ToLower() == AttackName.ToLower());

            if (attack != null)
            {
                attack.DamageModifier = new DamageModifier(NewDamage, Turns);
            }
        }

        public void Process(GameField game, Player caster, Player opponent, PokemonCard pokemonSource)
        {
            ApplyModifier(caster.ActivePokemonCard);
        }
    }
}
